using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IRecipeTagRepository : IRepository<RecipeTag>
    {
        Task<Response<List<RecipeTag>>> GetByRecipeIdEagerAsync(int recipeId);
    }
}
