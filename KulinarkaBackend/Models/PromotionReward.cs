using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class PromotionReward
    {
        [Key]
        public int TitleId { get; set; }
        [JsonIgnore]
        public virtual Title? Title { get; set; }
        public int DurationInDays { get; set; }
        public int IntervalInDays { get; set; }
        public int PostsToPromote { get; set; }
    }
}
