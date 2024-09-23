using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class AchievementDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PointsNeeded { get; set; }
        public string IconBase64 { get; set; }
        public RequirementType RequirementType { get; set; }
    }
}
