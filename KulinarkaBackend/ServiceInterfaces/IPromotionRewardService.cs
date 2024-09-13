using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IPromotionRewardService
    {
        Task<Response<PromotionReward>> GetByTitleId(int userTitleId);
    }
}
