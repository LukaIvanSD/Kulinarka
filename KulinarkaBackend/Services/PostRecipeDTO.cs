using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Services
{
    public class PostRecipeDTO
    {
        public Recipe recipe { get; set; }
        public List<RecipeIngredientDTO> RecipeIngredientDTOs { get; set; }
        public List<PreparationStep> PreparationSteps { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
