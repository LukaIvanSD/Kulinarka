using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IRecipeIngredientService
    {
        Task<Response<List<RecipeIngredient>>> AddAsync(int recipeId, List<RecipeIngredientDTO> recipeIngredientDTOs,bool saveChanges=true);
        Task<Response<List<RecipeIngredient>>> GetRecipeIngredientsAsync(int recipeId);
        Task<Response<List<RecipeIngredient>>> UpdateAsync(List<RecipeIngredient> oldRecipeIngredients, List<RecipeIngredientDTO> newRecipeIngredients, bool saveChanges=true);
    }
}
