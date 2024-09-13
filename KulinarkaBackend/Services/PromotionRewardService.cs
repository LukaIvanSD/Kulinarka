using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class PromotionRewardService : IPromotionRewardService
    {
        private readonly IPromotionRewardRepository promotionRewardRepository;
        public PromotionRewardService(IPromotionRewardRepository promotionRewardRepository)
        {
            this.promotionRewardRepository = promotionRewardRepository;
        }
        public async Task<Response<PromotionReward>> GetByTitleId(int userTitleId)
        {
            return await promotionRewardRepository.GetByTitleId(userTitleId);
        }
    }
}
