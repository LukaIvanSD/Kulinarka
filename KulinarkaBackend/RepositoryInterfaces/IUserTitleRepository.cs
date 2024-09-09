using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IUserTitleRepository:IRepository<UserTitle>
    {
        public Task<Response<UserTitle>> GetUserTitleEagerAsync(int userId);
        Task<Response<UserTitle>> UpdateAsync(UserTitle userTitle);
    }
}
