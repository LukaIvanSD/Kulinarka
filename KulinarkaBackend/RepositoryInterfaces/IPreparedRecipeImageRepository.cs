using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IPreparedRecipeImageRepository : IRepository<PreparedRecipeImage>
    {
        Task<Response<List<PreparedRecipeImage>>> GetByRecipeIdPagedAsync(int recipeId,int startIndex,int resultCount);
        Task<Response<List<PreparedRecipeImage>>> GetByUserAndRecipeIdAsync(int userId, int recipeId);
        Task<Response<List<PreparedRecipeImage>>> GetByUserIdPagedAsync(int userId, int startIndex, int resultCount);
    }
}
