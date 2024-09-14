
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required(ErrorMessage ="Name of recipe is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
        //[Required(ErrorMessage ="Picture of recipe is required")]
        public byte[]? Picture { get; set; }

        public byte[]? VideoData { get; set; }
        [MaxLength(100)]
        public string? ContentType { get; set; }

        [Required(ErrorMessage ="Duration is required")]
        public TimeSpan Duration { get; set; }
        [Required(ErrorMessage = "Difficulty is required")]
        public Difficulty Difficulty { get; set; }
        [Required(ErrorMessage ="Number of people is required")]
        public int NumberOfPeople { get; set; }
        public string? ChefsAdvice { get; set; }
        public DateTime CreationDate { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<PromotionRewardRecipe>? Promotions { get; set; }
        [JsonIgnore]
        public virtual ICollection<RecipeIngredient>? Ingredients { get; set; }
        [JsonIgnore]
        public virtual ICollection<RecipeTag>? Tags { get; set; }
        [JsonIgnore]
        public virtual ICollection<PreparationStep>? PreparationSteps { get; set; }

        internal DateTime? DatePromoted()
        {
            if (Promotions == null)
                return null;
            foreach (PromotionRewardRecipe userReward in Promotions)
                if (userReward.IsActive())
                    return userReward.DateUsed;
            return null;
        }

        internal bool IsPromoted()
        {
            if (Promotions == null)
                return false;
            foreach (PromotionRewardRecipe userReward in Promotions)
                if (userReward.IsActive())
                    return true;
            return false;
        }
        internal bool WasPromotedInInterval()
        {
            if (Promotions == null)
                return false;
            foreach (PromotionRewardRecipe promotionRewardRecipe in Promotions)
                if (promotionRewardRecipe.IsInInterval())
                    return true;
            return false;
        }
        internal PromotionRewardRecipe GetActivePromotion()
        {
            if (Promotions == null)
                return null;
            foreach (PromotionRewardRecipe promotionRewardRecipe in Promotions)
                if (promotionRewardRecipe.IsActive())
                    return promotionRewardRecipe;
            return null;
        }

        internal void UpdatePromotions(int newPromotionRewardId)
        {
            if (Promotions == null)
                return;
            foreach (PromotionRewardRecipe promotionRewardRecipe in Promotions)
                promotionRewardRecipe.UpdateReward(newPromotionRewardId);
        }

        internal bool IsUserOwnerOfRecipe(int userId)
        {
            return UserId == userId;
        }
    }
}
