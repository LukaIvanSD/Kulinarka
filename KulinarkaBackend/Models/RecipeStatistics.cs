using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class RecipeStatistics
    {
        [Key]
        public int RecipeId { get; set; }
        public int Likes { get; set; }
        public int Views { get; set; }
        public int Shares { get; set; }
        public int Favorites { get; set; }
        public int Comments { get; set; }
        public double AverageRating { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }

        internal bool AddComment()
        {
            Comments++;
            return true;
        }
    }
}
