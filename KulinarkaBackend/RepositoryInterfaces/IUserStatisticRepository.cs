using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IUserStatisticRepository : IRepository<UserStatistic>
    {
        Task<Response<UserStatistic>> UpdateAsync(UserStatistic userStatistic,bool saveChanges=true);
    }
}
