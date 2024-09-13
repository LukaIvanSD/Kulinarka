﻿using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IPromotionRewardRepository : IRepository<PromotionReward>
    {
        Task<Response<PromotionReward>> GetByTitleId(int userTitleId);
    }
}
