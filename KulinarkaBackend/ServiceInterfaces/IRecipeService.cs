using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.Interfaces
{
    public interface IRecipeService
    {
        Task<Response<List<Recipe>>> GetAllAsync();
        Task<Response<Recipe>> GetByIdAsync(int id);
        Task<Response<Recipe>> AddAsync(User user, Recipe recipe);
        Task<Response<Recipe>> UpdateAsync(User user, Recipe recipe);
        Task<Response<Recipe>> DeleteAsync(User user, int id);
        Task<Response<List<SortedRecipesDTO>>> GetSortedAsync();
        Task<Response<List<SortedRecipesDTO>>> GetUserRecipesAsync(User user);
        Task<Response<List<Recipe>>> GetUserRecipesWithPromotionsEagerAsync(User user);
    }
}
