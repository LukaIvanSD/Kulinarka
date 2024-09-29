using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Response<List<Comment>>> GetByRecipeIdPagedAsync(int recipeId, int startIndex, int resultSize);
        Task<Response<List<Comment>>> GetByRecipeIdPagedWithCreatorEagerAsync(int recipeId, int startIndex, int resultSize);
    }
}
