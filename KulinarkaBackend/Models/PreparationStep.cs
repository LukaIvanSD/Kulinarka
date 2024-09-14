using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class PreparationStep
    {
        [Key]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        [Required]
        public int SequenceNum { get; set; }
        [Required]
        public string Description { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }

        internal bool IsSequenceValid(int maxSteps)
        {
            return SequenceNum > 0 && SequenceNum <= maxSteps;
        }
    }
}
