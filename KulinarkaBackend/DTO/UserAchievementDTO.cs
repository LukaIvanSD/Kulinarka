using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class UserAchievementDTO
    {
        public AchievementDTO Achievement { get; set; }
        public int PointsCollected { get; set; }
        public DateTime? AquiredDate { get; set; }
        public UserAchievementDTO()
        {
        }

    }
}
