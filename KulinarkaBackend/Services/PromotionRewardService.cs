using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class PromotionRewardService : IPromotionRewardRecipeService
    {
        private readonly IPromotionRewardRecipeRepository promotionRewardRecipeRepository;
        private readonly IRecipeService recipeService;
        private readonly IUserTitleService userTitleService;
        public PromotionRewardService(IPromotionRewardRecipeRepository promotionRewardRecipeRepository,IRecipeService recipeService,IUserTitleService userTitleService) {
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
