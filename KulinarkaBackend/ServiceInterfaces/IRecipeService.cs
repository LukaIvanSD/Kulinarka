using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.Interfaces
{
    public interface IRecipeService
    {
        Task<Response<List<Recipe>>> GetRecipesAsync();
        Task<Response<Recipe>> GetRecipeAsync(int id);
        Task<Response<Recipe>> AddRecipeAsync(User user, Recipe recipe);
        Task<Response<Recipe>> UpdateRecipeAsync(User user, Recipe recipe);
        Task<Response<Recipe>> DeleteRecipeAsync(User user, int id);
    }
}
