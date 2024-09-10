using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class UserReward
    {
        public int RecipeId { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }
        public DateTime DateUsed { get; set; }
    }
}
