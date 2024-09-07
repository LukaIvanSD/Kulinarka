using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class UserAchievement
    {

        public int UserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual User? User { get; set; }
        public int AchievementId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public virtual Achievement? Achievement { get; set; }
        public DateTime? AquiredDate { get; set; }
        public int PointsCollected { get; set; }

        public UserAchievement(int achievementId, int userId)
        {
            AchievementId = achievementId;
            UserId = userId;
            PointsCollected = 0;
        }
    }
}
