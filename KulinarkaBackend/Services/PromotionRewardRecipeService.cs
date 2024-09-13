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
        public PromotionRewardRecipeService(IPromotionRewardRecipeRepository promotionRewardRecipeRepository,IPromotionRewardService promotionRewardService,IRecipeService recipeService,IUserTitleService userTitleService) {
            this.promotionRewardRecipeRepository = promotionRewardRecipeRepository;
            this.promotionRewardService = promotionRewardService;
            this.recipeService = recipeService;
            this.userTitleService = userTitleService;
        }

        public async Task<Response<PromotionRewardRecipe>> PromoteRecipeAsync(User user, int recipeId)
        {
            var recipe = await recipeService.GetByIdAsync(recipeId);
            if(!recipe.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(recipe.ErrorMessage, recipe.StatusCode);
            if (!recipe.Data.IsUserOwnerOfRecipe(user.Id))
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

        public async Task<Response<PromotionRewardRecipe>> UpdateUserPromotions(User user,int newTitleId)
        {
            var result = await recipeService.GetUserRecipesWithPromotionsEagerAsync(user);
            if (!result.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(result.ErrorMessage, result.StatusCode);
            user.Recipes = result.Data;
            var newPromotionRewardResult = await promotionRewardService.GetByTitleId(newTitleId);
            if (!newPromotionRewardResult.IsSuccess)
                return Response<PromotionRewardRecipe>.Failure(newPromotionRewardResult.ErrorMessage, newPromotionRewardResult.StatusCode);
            PromotionReward newPromotionReward = newPromotionRewardResult.Data;
            user.UpdateRecipesPromotion(newPromotionReward.Id);
            if (user.IsDemoted())
            {
                var removeResult = await RemoveExcessPromotions(user, newPromotionReward);
                if (!removeResult.IsSuccess)
                    return Response<PromotionRewardRecipe>.Failure(removeResult.ErrorMessage, removeResult.StatusCode);
            }
            return Response<PromotionRewardRecipe>.Success(null, StatusCode.OK);
        }

        private async Task<Response<PromotionRewardRecipe>> RemoveExcessPromotions(User user,PromotionReward newPromotionReward)
        {
            List<PromotionRewardRecipe> activePromotions = user.GetActivePromotions();
            activePromotions = activePromotions.OrderBy(prr => prr.DateUsed).ToList();
            while (activePromotions.Count > newPromotionReward.PostsToPromote)
            {
                var deleteResult = await promotionRewardRecipeRepository.DeleteAsync(activePromotions[0],false);
                if (!deleteResult.IsSuccess)
                    return Response<PromotionRewardRecipe>.Failure(deleteResult.ErrorMessage, deleteResult.StatusCode);
                activePromotions.RemoveAt(0);
            }
            return Response<PromotionRewardRecipe>.Success(null, StatusCode.OK);
        }

        private async Task<Response<PromotionRewardRecipe>> SaveUserPromotionsToDb(User user)
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
            return Response<PromotionRewardRecipe>.Success(null, StatusCode.OK);
        }

        private async Task<Response<PromotionRewardRecipe>> CreatePromotion(User user,Recipe recipe)
        {
            PromotionRewardRecipe promotionRewardRecipe = new PromotionRewardRecipe(user.UserTitle.CurrentTitle.PromotionReward.Id,recipe.Id);
            return await promotionRewardRecipeRepository.CreateAsync(promotionRewardRecipe);
        }
    }
}
