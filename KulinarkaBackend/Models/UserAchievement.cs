using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class UserAchievement
    {

        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
        public int AchievementId { get; set; }
        [JsonIgnore]
        public virtual Achievement? Achievement { get; set; }
        public DateTime? AquiredDate { get; set; }
        public int PointsCollected { get; set; }

        public UserAchievement(int achievementId, int userId)
        {
            AchievementId = achievementId;
            UserId = userId;
            PointsCollected = 0;
        }
        public UserAchievement()
        {
        }
        public bool AddPoint()
        {
            if (AquiredDate != null)
                return false;
            PointsCollected++;
            return true;
        }
        public bool IsCompleted()
        {
            return AquiredDate != null;
        }

        internal bool IsJustCompleted()
        {
            if (PointsCollected >= Achievement.PointsNeeded && AquiredDate==null)
            {
                AquiredDate = DateTime.Now;
                return true;
            }
            return false;

        }

        internal bool RemovePoint()
        {
            if (PointsCollected == 0)
                return false;
            PointsCollected--;
            return true;

        }

        internal bool IsRevoked()
        {
            if (PointsCollected < Achievement.PointsNeeded && AquiredDate != null)
            {
                AquiredDate = null;
                return true;
            }
            return false;
        }
    }
}
