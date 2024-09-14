using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public enum TagType
    {
        Difficulty,
        PeopleNumber,
        PreparationTime,
        PreparationMethod,
        MealTime,
        Flavor
    }
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public TagType TagType { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public  virtual ICollection<RecipeTag>? RecipeTags { get; set; }
        public Tag(TagType tagType, string name)
        {
            TagType = tagType;
            Name = name;
        }
    }
}
