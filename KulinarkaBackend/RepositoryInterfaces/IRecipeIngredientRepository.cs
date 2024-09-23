using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IRecipeIngredientRepository : IRepository<RecipeIngredient>
    {
        Task<Response<List<RecipeIngredient>>> GetByRecipeIdEagerAsync(int recipeId);
    }
}
