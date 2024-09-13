using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IPromotionRewardRecipeRepository : IRepository<PromotionRewardRecipe>
    {
        Task<Response<PromotionRewardRecipe>> UpdateAsync(PromotionRewardRecipe entity, bool saveChanges = true);
        Task<Response<PromotionRewardRecipe>> DeleteAsync(PromotionRewardRecipe entity, bool saveChanges = true);
    }
}
