using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface ITitleRepository : IRepository<Title>
    {
        Task<Response<Title>> GetNextTitleAndRewardsEagerAsync(int nextId);
    }
}
