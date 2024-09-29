
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IRecipeStatisticsService
    {
        Task<Response<RecipeStatistics>> AddAsync(int recipeId,bool saveChanges=true);
        Task<Response<RecipeStatistics>> AddCommentStatisticAsync(int recipeId,bool saveChanges = true);
    }
}
