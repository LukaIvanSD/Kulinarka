using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IRecipeTagService
    {
        Task<Response<List<RecipeTag>>> AddAsync(int recipeId, List<Tag> tags);
        Task<Response<List<RecipeTag>>> GetByRecipeIdAsync(int recipeId);
    }
}
