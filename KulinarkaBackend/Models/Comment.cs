using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int CreatorId { get; set; }
        [Required(ErrorMessage ="Comment is required")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Header is required")]
        public string Header { get; set; }
        public DateTime DateCreated { get; set; }
        [JsonIgnore]
        public virtual User? Creator { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }
    }
}
