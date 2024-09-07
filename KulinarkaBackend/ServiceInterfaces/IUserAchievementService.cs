using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IUserAchievementService
    {
        Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId);
    }
}
