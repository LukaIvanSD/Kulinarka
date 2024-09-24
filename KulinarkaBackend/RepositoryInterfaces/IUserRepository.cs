using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IUserRepository:IRepository<User>
    {
        Task<Response<User>> GetByUsernameAsync(string username);
        Task<bool> IsUserUnique(User user);
        Task<Response<User>> GetUserAchievementsEagerAsync(int id);
        Task<Response<User>> GetUserTitleAndStatisticAndRewardsEagerAsync(int userId);
        Task<Response<User>> GetUserAndTitleEagerAsync(int userId);
    }
}
