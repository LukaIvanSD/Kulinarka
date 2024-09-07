

using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public enum RequirementType
    {
        Followers,
        Likes,
        Recipes
    }
    public class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int PointsNeeded { get; set; }
        public byte[]? Icon { get; set; }
        public RequirementType RequirementType { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserAchievement>? UserAchievements { get; set; }
    }
}
