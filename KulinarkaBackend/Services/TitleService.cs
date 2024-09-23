using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class TitleService :ITitleService
    {
        private readonly ITitleRepository titleRepository;
        public TitleService(ITitleRepository titleRepository)
        {
            this.titleRepository = titleRepository;
        }

        public async Task<Response<Title>> GetNextTitle(int currentTitleId)
        {
            int nextId = Title.GetNextTitleId(currentTitleId);
            var result = await titleRepository.GetByIdAsync(nextId);
            if (!result.IsSuccess)
                return Response<Title>.Success(null, StatusCode.NotFound);
            return result;
        }
        public async Task<Response<Title>> GetNextTitleAndRewardsEagerAsync(int currentTitleId)
        {
            int nextId = Title.GetNextTitleId(currentTitleId);
            var result = await titleRepository.GetNextTitleAndRewardsEagerAsync(nextId);
            return result;
        }
    }
}
