using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class PostRecipeDTO
    {
        public Recipe recipe { get; set; }
        public List<RecipeIngredientDTO> RecipeIngredientDTOs { get; set; }
        public List<PreparationStepDTO> PreparationSteps { get; set; }
        public List<Tag> Tags { get; set; }
        public PostRecipeDTO(){}
        public PostRecipeDTO(Recipe recipe, List<RecipeIngredientDTO> RecipeIngredientDTOs, List<PreparationStepDTO> PreparationSteps, List<Tag> Tags)
        {
            this.recipe = recipe;
            this.RecipeIngredientDTOs = RecipeIngredientDTOs;
            this.PreparationSteps = PreparationSteps;
            this.Tags = Tags;
        }
    }
}
