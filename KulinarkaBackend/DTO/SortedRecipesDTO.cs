using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class SortedRecipesDTO
    {
        public Recipe Recipe { get; set; }
        public bool IsPromoted { get; set; }
        public SortedRecipesDTO(Recipe recipe , bool isPromoted) {
            Recipe = recipe;
            IsPromoted = isPromoted;
        }
    }
}
