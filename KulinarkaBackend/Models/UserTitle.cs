

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kulinarka.Models
{
    public class UserTitle
    {
        [Key]
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        public int TitleId { get; set; }
        [JsonIgnore]
        public virtual Title CurrentTitle { get; set; }
        [JsonIgnore]
        [NotMapped]
        public virtual Title NextTitle { get; set; }
        public DateTime AquiredDate { get; set; }

        public UserTitle(int userId)
        {
            UserId = userId;
            TitleId = 1;
            AquiredDate = DateTime.Now;
        }


        public bool HasEnoughAchievements(int achievements)
        {
            return NextTitle.AchievementsRequired <= achievements;
        }

        public bool Promote(int achievements)
        {
            if (!HasEnoughAchievements(achievements))
                return false;
            TitleId = NextTitle.Id;
            AquiredDate = DateTime.Now;
            return true;
        }

        internal bool Demote(int achievements)
        {
           if(CurrentTitle.AchievementsRequired<=achievements)
                return false;
           TitleId=Title.GetPreviousTitle(CurrentTitle.Id);
           AquiredDate = DateTime.Now;
           return true;
        }
    }
}
