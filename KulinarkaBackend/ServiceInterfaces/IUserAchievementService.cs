using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IUserAchievementService
    {
        Task<Response<List<UserAchievement>>> AddProgress(int userId, RequirementType requirementType);
        Task<Response<List<UserAchievement>>> CreateUserAchievements(User user);
        Task<Response<Achievement>> CreateUsersAchievement(Achievement achievements);
        Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId);
        Task<Response<List<UserAchievement>>> RemoveProgress(int userId, RequirementType requirementType);
    }
}
