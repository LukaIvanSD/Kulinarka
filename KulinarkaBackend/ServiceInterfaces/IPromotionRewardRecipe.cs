using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IPromotionRewardRecipeService
    {
        Task<Response<PromotionRewardRecipe>> PromoteRecipeAsync(User user, int recipeId);
        Task<Response<PromotionRewardRecipe>> UpdateUserPromotions(User user);
    }
}
