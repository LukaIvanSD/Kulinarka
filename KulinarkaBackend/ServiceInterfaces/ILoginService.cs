using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.Interfaces
{
    public interface ILoginService
    {
        Task<Response<User>> LogInWithCookieAsync(string username);
        Task<Response<User>> LogInAsync(string username, string password, bool rememberMe);
        Task<Response<User>> LogOutAsync();
        Task<Response<User>> GetSessionAsync();
    }
}
