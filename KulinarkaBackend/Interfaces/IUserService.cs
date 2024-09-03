using Kulinarka.Models;

namespace Kulinarka.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> GetUsersAsync();
        public Task<bool>DeleteUserAsync(int id);
        public Task<bool> UpdateUserAsync(User user);
        public Task<User> RegisterUserAsync(User user);
        public Task<User> GetUserAsync(int id);
        public Task<bool> UsernameExistsAsync(string username);
        public Task<bool> EmailExistsAsync(string email);
        public Task<bool> IsUserUnique(User user);

    }
}
