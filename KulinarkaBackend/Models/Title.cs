

using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public enum TitleType
    {
        Chef,
        ProChef,
        MasterChef,

    }
    public class Title
    {
        public int Id { get; set; }
        public TitleType TitleType { get; set; }
        public int AchievementsRequired { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserTitle> UserTitles { get; set; }

        internal static int GetNextTitleId(int currentTitleId)
        {
            return currentTitleId + 1;
        }

        internal static int GetPreviousTitle(int currentTitleId)
        {
            return currentTitleId - 1;
        }
    }
}
