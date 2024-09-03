using Kulinarka.Models;

namespace Kulinarka.Interfaces
{
    public interface ILoginService
    {
        Task<bool> IsLoggedInAsync();
        Task LogInWithCookieAsync(string username);
        Task<User> LogInAsync(string username, string password, bool rememberMe);
        Task<bool> LogOutAsync();
        Task<User> GetSessionAsync();
    }
}
