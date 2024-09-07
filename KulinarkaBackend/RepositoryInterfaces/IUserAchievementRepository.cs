﻿using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IUserAchievementRepository: IRepository<UserAchievement>
    {
        Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId);
    }
}
