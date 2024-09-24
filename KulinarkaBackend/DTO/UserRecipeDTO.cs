using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class UserRecipeDTO
    {
        public RecipeInfoDTO Recipe { get; set; }
        public List<RecipeIngredientDTO> Ingredients { get; set; }
        public List<Tag> Tags { get; set; }
        public bool IsPromoted { get; set; }
    }
}
