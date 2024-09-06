using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IUserRepository:IRepository<User>
    {
        Task<Response<User>> GetByUsernameAsync(string username);
        Task<bool> IsUserUnique(User user);
    }
}
