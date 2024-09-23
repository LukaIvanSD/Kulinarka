using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class ProfileDTO
    {
        public UserInfoDTO UserInfo { get; set; }
        public TitleDTO Title { get; set; }
        public UserStatisticDTO UserStatistics { get; set; }
        public PromotionRewardDTO CurrentPromotionReward { get; set; }
        public PromotionRewardDTO NextPromotionReward { get; set; }
        public ProfileDTO() { }
        public ProfileDTO(UserInfoDTO userInfoDTO, TitleDTO titleDTO, UserStatisticDTO userStatisticDTO, PromotionRewardDTO currentPromotionReward, PromotionRewardDTO nextPromotionReward)
        {
            UserInfo = userInfoDTO;
            Title = titleDTO;
            UserStatistics = userStatisticDTO;
            CurrentPromotionReward = currentPromotionReward;
            NextPromotionReward = nextPromotionReward;
        }


    }
}
