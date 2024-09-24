using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IRecipeDetailsService
    {
        Task<Response<RecipeDetailsDTO>> GetRecipeDetails(int recipeId);
        Task<Response<RecipeDetailsDTO>> UpdateRecipeDetailsAsync(User user, RecipeDetailsDTO recipeDetailsDTO);
    }
}
