using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IRecipeIngredientService
    {
        Task<Response<List<RecipeIngredient>>> AddAsync(int recipeId, List<RecipeIngredientDTO> recipeIngredientDTOs);
    }
}
