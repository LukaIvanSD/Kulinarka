using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class RecipeDetailsDTO
    {
        public RecipeDetailsInfoDTO Recipe { get; set; }
        public List<RecipeIngredientDTO> Ingredients { get; set; }
        public List<Tag> Tags { get; set; }
        public List<PreparationStepDTO> Steps { get; set; }
        public UserRecipeDetailsDTO Owner { get; set; }
        public bool IsPromoted { get; set; }
    }
}
