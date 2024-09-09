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

    }
}
