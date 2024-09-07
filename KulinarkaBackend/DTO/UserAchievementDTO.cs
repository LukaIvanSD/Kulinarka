using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class UserAchievementDTO
    {
        public Achievement Achievement { get; set; }
        public int PointsCollected { get; set; }
        public DateTime? AquiredDate { get; set; }
        public UserAchievementDTO(UserAchievement userAchievement)
        {
            Achievement = userAchievement.Achievement;
            PointsCollected = userAchievement.PointsCollected;
            AquiredDate = userAchievement.AquiredDate;
        }
        public UserAchievementDTO()
        {
        }

    }
}
