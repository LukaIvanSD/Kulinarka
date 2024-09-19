using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class SortedRecipesDTO
    {
        public int Id { get; set; }
        public string OwnerUsername { get; set; }
        public string PictureBase64 { get; set; }
        public string Name { get; set; }
        public bool IsPromoted { get; set; }
        public SortedRecipesDTO(Recipe recipe , bool isPromoted, string ownerUsername)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            PictureBase64 = Convert.ToBase64String(recipe.Picture);
            IsPromoted = isPromoted;
            this.OwnerUsername = ownerUsername;
        }
    }
}
