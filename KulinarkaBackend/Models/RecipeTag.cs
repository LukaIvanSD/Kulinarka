using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class RecipeTag
    {
        [Key]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }
        public int TagId { get; set; }
        [JsonIgnore]
        public virtual Tag? Tag { get; set; }
        public RecipeTag(int recipeId, int tagId)
        {
            RecipeId = recipeId;
            TagId = tagId;
        }
    }
}
