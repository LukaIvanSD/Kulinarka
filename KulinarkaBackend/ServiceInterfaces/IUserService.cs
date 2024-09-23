using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.Interfaces
{
    public interface IUserService
    {
        public Task<Response<List<User>>> GetUsersAsync();
        public Task<Response<User>>DeleteUserAsync(int id);
        public Task<Response<User>> UpdateUserAsync(User loggedInUser,User user);
        public Task<Response<User>> RegisterUserAsync(User user);
        public Task<Response<User>> GetUserByIdAsync(int id);
        public Task<Response<User>> GetUserAchievementsEagerAsync(int id);
        Task<Response<User>> GetByUsernameAsync(string username);
        Task<Response<User>> BeginTransactionAsync();
        Task<Response<User>> SaveChangesAsync();
        Task<Response<User>> CommitTransactionAsync();
        Task<Response<User>> RollbackTransactionAsync();
        Task<Response<User>> UpdateAsync(User user, bool saveChanges=true);
        Task<Response<User>> GetUserTitleAndStatisticAndRewardsEagerAsync(int userId);
    }
}
