using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IRecipeTagService
    {
        Task<Response<List<RecipeTag>>> AddAsync(int recipeId, List<Tag> tags,bool saveChanges=true);
        Task<Response<List<RecipeTag>>> GetByRecipeIdAsync(int recipeId);
        Task<Response<List<RecipeTag>>> UpdateAsync(List<RecipeTag> oldRecipeTags, List<Tag> newTags, bool saveChanges=true);
        Task<Response<List<RecipeTag>>> DeleteTags(List<RecipeTag> recipeTags, bool saveChanges = true);

    }
}
