using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Kulinarka.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="First name is required") ]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Last name is required")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Gender is required")]
        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }

        [Required(ErrorMessage ="Birthday is required")]
        public DateOnly Birthday { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [Required(ErrorMessage ="E-mail is required")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Username is required")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [MaxLength(100)]
        public string Password { get; set; }

        public DateTime DateOfCreation { get; set; }
        public string? Bio { get; set; }

        public byte[]? Picture { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserAchievement>? UserAchievements { get; set; }
        [JsonIgnore]
        public virtual UserTitle? UserTitle { get; set; }
        [JsonIgnore]
        public virtual UserStatistic? UserStatistic { get; set; }
        [JsonIgnore]
        public virtual ICollection<Recipe>? Recipes { get; set; }

        public int AddPoint(RequirementType requirementType)
        {
            int achievementsJustCompleted = 0;
            foreach (UserAchievement userAchievement in UserAchievements)
            {
                if (userAchievement.Achievement.RequirementType == requirementType)
                { 
                    userAchievement.AddPoint();
                    if (userAchievement.IsJustCompleted())
                        achievementsJustCompleted++;
                }
            }
            return achievementsJustCompleted;
        }
        public PromotionReward GetTitleRewards() 
        { 
            return UserTitle.CurrentTitle.PromotionReward;
        }
        public PromotionReward GetNextTitleRewards()
        {
            if(UserTitle.NextTitle!=null)
                return UserTitle.NextTitle.PromotionReward;
            return null;
        }

        internal bool CanPromote(int recipeId)
        {
            if(IsPromotionActive(recipeId))
                return false;
            if (UserTitle.CurrentTitle.PromotionReward.PostsToPromote > PromotionsInInterval())
                return true;
            return false;
        }

        private bool IsPromotionActive(int recipeId)
        {
            return Recipes.Where(r => r.Id == recipeId).FirstOrDefault().IsPromoted();
        }

        private int PromotionsInInterval()
        {
            int promotionsInInterval = 0;
            foreach (Recipe recipe in Recipes)
            {
                if (recipe.WasPromotedInInterval())
                    promotionsInInterval++;
            }
            return promotionsInInterval;
        }

        internal int RemovePoint(RequirementType requirementType)
        {
            int achievementsJustRevoked = 0;
            foreach (UserAchievement userAchievement in UserAchievements)
            {
                if (userAchievement.Achievement.RequirementType == requirementType)
                {
                    if(userAchievement.RemovePoint())
                        if (userAchievement.IsRevoked())
                            achievementsJustRevoked++;
                }
            }
            return achievementsJustRevoked;
        }

        internal List<PromotionRewardRecipe> GetActivePromotions()
        {
            List<PromotionRewardRecipe> activePromotions = new List<PromotionRewardRecipe>();
            foreach (Recipe recipe in Recipes)
            { 
                PromotionRewardRecipe activePromotion= recipe.GetActivePromotion();
                if (activePromotion!=null)
                    activePromotions.Add(activePromotion);
            }
            return activePromotions;
        }

        internal bool IsDemoted()
        {
            return UserTitle.TitleId < UserTitle.CurrentTitle.Id;
        }

        internal void UpdateRecipesPromotion(int newPromotionRewardId)
        {
            foreach (Recipe recipe in Recipes)
                recipe.UpdatePromotions(newPromotionRewardId);
        }
    }
}
