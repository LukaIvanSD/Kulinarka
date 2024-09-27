using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class PreparedRecipeImage
    {
        [Key]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int CreatorId { get; set; }
        [Required(ErrorMessage = "Date uploaded is required")]
        public DateTime DateUploaded { get; set; }
        [Required(ErrorMessage ="Image is required")]
        public byte[] Image { get; set; }
        [JsonIgnore]
        public virtual User? Creator { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }
        public PreparedRecipeImage()
        {

        }
    }
}
