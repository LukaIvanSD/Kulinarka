using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class UserStatistic
    {
        [Key]
        public int UserId { get; set; }
        public int Followers { get; set; }
        public float AverageRating { get; set; }
        public int Likes { get; set; }
        public int Favorites { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public UserStatistic(int userId) {
            UserId = userId;
            Followers = 0;
            AverageRating = 0;
            Likes = 0;
            Favorites = 0;
        }
    }
}
