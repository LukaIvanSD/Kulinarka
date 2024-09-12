using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class PromotionRewardRecipeService : IPromotionRewardRecipeService
    {
        private readonly IPromotionRewardRecipeRepository promotionRewardRecipeRepository;
        private readonly IPromotionRewardService promotionRewardService;
        private readonly IRecipeService recipeService;
        private readonly IUserTitleService userTitleService;
        public PromotionRewardRecipeService(IPromotionRewardRecipeRepository promotionRewardRecipeRepository,IRecipeService recipeService,IUserTitleService userTitleService) {
            this.promotionRewardRecipeRepository = promotionRewardRecipeRepository;
            this.recipeService = recipeService;
            this.userTitleService = userTitleService;
        }

        public async Task<Response<PromotionRewardRecipe>> PromoteRecipeAsync(User user, int recipeId)
        {
            var recipe = await recipeService.GetByIdAsync(recipeId);
            if(!recipe.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(recipe.ErrorMessage, recipe.StatusCode);
            if (!IsUserOwnerOfRecipe(user.Id,recipe.Data))
                return Response<PromotionRewardRecipe>.Failure("User is not owner of recipe",StatusCode.BadRequest);
            var recipesResult = await recipeService.GetUserRecipesWithPromotionsEagerAsync(user);
            if (!recipesResult.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(recipesResult.ErrorMessage, recipesResult.StatusCode);
            user.Recipes= recipesResult.Data;
            var userTitleResult= await userTitleService.GetUserTitleWithPromotionRewardEagerAsync(user.Id);
            if(!userTitleResult.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(userTitleResult.ErrorMessage, StatusCode.BadRequest);
            user.UserTitle = userTitleResult.Data;
            if(!user.CanPromote(recipeId))
                return Response<PromotionRewardRecipe>.Failure("User can't promote recipe", StatusCode.BadRequest);
            return await CreatePromotion(user,recipe.Data);
        }

        public async Task<Response<PromotionRewardRecipe>> UpdateUserPromotions(User user)
        {
            var result = await recipeService.GetUserRecipesWithPromotionsEagerAsync(user);
            if (!result.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(result.ErrorMessage, result.StatusCode);
            user.Recipes = result.Data;
            var promotionRewardResult = await promotionRewardService.GetByTitleId(user.UserTitle.TitleId);
            if (!promotionRewardResult.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(promotionRewardResult.ErrorMessage, promotionRewardResult.StatusCode);
            PromotionReward newPromotionReward = promotionRewardResult.Data;
            if (user.IsPromoted())
            {
                foreach (Recipe recipe in user.Recipes)
                {
                    if (recipe.Promotions == null)
                        continue;
                    foreach (PromotionRewardRecipe promotionRewardRecipe in recipe.Promotions)
                            promotionRewardRecipe.UpdateReward(newPromotionReward.Id);
                }
            }
            else
            {
                foreach (Recipe recipe in user.Recipes)
                {
                    if (recipe.Promotions == null)
                        continue;
                    foreach (PromotionRewardRecipe promotionRewardRecipe in recipe.Promotions)
                        promotionRewardRecipe.UpdateReward(newPromotionReward.Id);
                }
                List<PromotionRewardRecipe> activePromotions = user.GetActivePromotions();
                activePromotions = activePromotions.OrderBy(prr => prr.DateUsed).ToList();
                while (activePromotions.Count > newPromotionReward.PostsToPromote)
                {
                    await promotionRewardRecipeRepository.DeleteAsync(activePromotions[0]);
                    activePromotions.RemoveAt(0);
                }
            }
            return await SaveUserPromotionsToDb(user);
        }

        private async Task<Response<PromotionRewardRecipe>> SaveUserPromotionsToDb(User user)
        {
            var transactionResult = await promotionRewardRecipeRepository.BeginTransactionAsync();
            if (!transactionResult.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(transactionResult.ErrorMessage, StatusCode.InternalServerError);
            try
            {
                foreach (Recipe recipe in user.Recipes)
                {
                    if (recipe.Promotions == null)
                        continue;
                    foreach (PromotionRewardRecipe promotionRewardRecipe in recipe.Promotions)
                    {
                        var result = await promotionRewardRecipeRepository.UpdateAsync(promotionRewardRecipe,false);
                        if (!result.IsSuccess)
                            throw new Exception(result.ErrorMessage);
                    }
                }
                transactionResult = await promotionRewardRecipeRepository.SaveChangesAsync();
                if (!transactionResult.IsSuccess)
                    throw new Exception(transactionResult.ErrorMessage);
                transactionResult = await promotionRewardRecipeRepository.CommitTransactionAsync();
                if (!transactionResult.IsSuccess)
                    throw new Exception(transactionResult.ErrorMessage);
                return Response<PromotionRewardRecipe>.Success(null, StatusCode.OK);
            }
            catch (Exception ex)
            {
                await promotionRewardRecipeRepository.RollbackTransactionAsync();
                return Response<PromotionRewardRecipe>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        private async Task<Response<PromotionRewardRecipe>> CreatePromotion(User user,Recipe recipe)
        {
            PromotionRewardRecipe promotionRewardRecipe = new PromotionRewardRecipe(user.UserTitle.CurrentTitle.PromotionReward.Id,recipe.Id);
            return await promotionRewardRecipeRepository.CreateAsync(promotionRewardRecipe);
        }

        private bool IsUserOwnerOfRecipe(int userId,Recipe recipe)
        {
            return recipe.UserId == userId;
        }
    }
}
