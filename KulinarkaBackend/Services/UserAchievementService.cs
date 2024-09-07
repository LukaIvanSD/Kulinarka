using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class UserAchievementService : IUserAchievementService
    {
        private readonly IUserAchievementRepository userAchievementRepository;
        public UserAchievementService(IUserAchievementRepository userAchievementRepository)
        {
            this.userAchievementRepository = userAchievementRepository;
        }
        public async Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId)
        {
            return await userAchievementRepository.GetUserAchievementsEagerAsync(userId);
        }
    }
}
