using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<RecipeIngredient>? RecipeIngredients { get; set; }
        public Ingredient(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public Ingredient(string name) 
        {
            Name = name;
        }
    }
}
