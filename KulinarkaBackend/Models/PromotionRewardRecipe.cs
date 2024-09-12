﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public class PromotionRewardRecipe
    {
        public int RecipeId { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }
        public DateTime DateUsed { get; set; }
        public int PromotionRewardId { get; set; }
        [JsonIgnore]
        public virtual PromotionReward? PromotionReward { get; set; }

        public PromotionRewardRecipe(int promotionRewardId,int recipeId)
        {
            PromotionRewardId = promotionRewardId;
            RecipeId = recipeId;
            DateUsed = DateTime.Now;
        }
        internal bool IsActive()
        {
            return  LastPromotionDay() >= DateTime.Now;
        }
        private DateTime LastPromotionDay()
        {
            return DateUsed.AddDays(PromotionReward.DurationInDays);
        }
        //Check if the promotion is in the current month
        internal bool IsInInterval()
        {
            return DateUsed.AddDays(PromotionReward.IntervalInDays)>=DateTime.UtcNow;
        }
    }
}
