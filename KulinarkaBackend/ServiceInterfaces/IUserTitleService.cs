﻿using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IUserTitleService
    {
        public Task<Response<UserTitle>> GetUserTitleEagerAsync(int userId);
        Task<Response<UserTitle>> GetUserTitleWithPromotionRewardEagerAsync(int userId);
        public Task<Response<UserTitle>> UpdateUserTitle(User user);

    }
}
