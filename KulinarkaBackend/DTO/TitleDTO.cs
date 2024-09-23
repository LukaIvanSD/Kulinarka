namespace Kulinarka.DTO
{
    public class TitleDTO
    {
        public DateTime AquireDate { get; set; }
        public string Name { get; set; }
        public int CompletedAchievements { get; set; }
        public int NextTitleRequirement { get; set; }
        public int CurrentTitleRequirement { get; set; }
        public string NextTitleName { get; set; }
    }
}
