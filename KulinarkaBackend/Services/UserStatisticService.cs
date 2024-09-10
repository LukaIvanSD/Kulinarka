using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class UserStatisticService : IUserStatisticService
    {
        private readonly IUserStatisticRepository userStatisticRepository;
        public UserStatisticService(IUserStatisticRepository userStatisticRepository)
        {
            this.userStatisticRepository = userStatisticRepository;
        }
        public async Task<Response<UserStatistic>> GetUserStatisticAsync(int userId)
        {
            return await userStatisticRepository.GetByIdAsync(userId);
        }
        public async Task<Response<UserStatistic>> AddFollower(int userId) {
            var result = await userStatisticRepository.GetByIdAsync(userId);
            if (!result.IsSuccess)
                return result;
            result.Data.Followers++;
            return await userStatisticRepository.UpdateAsync(result.Data);
        }
        public async Task<Response<UserStatistic>> RemoveFollower(int userId)
        {
            var result = await userStatisticRepository.GetByIdAsync(userId);
            if (!result.IsSuccess)
                return result;
            result.Data.Followers--;
            return await userStatisticRepository.UpdateAsync(result.Data);
        }
        public async Task<Response<UserStatistic>> AddLike(int userId)
        {
            var result = await userStatisticRepository.GetByIdAsync(userId);
            if (!result.IsSuccess)
                return result;
            result.Data.Likes++;
            return await userStatisticRepository.UpdateAsync(result.Data);
        }
        public async Task<Response<UserStatistic>> RemoveLike(int userId)
        {
            var result = await userStatisticRepository.GetByIdAsync(userId);
            if (!result.IsSuccess)
                return result;
            result.Data.Likes--;
            return await userStatisticRepository.UpdateAsync(result.Data);
        }
        public async Task<Response<UserStatistic>> AddFavorite(int userId)
        {
            var result = await userStatisticRepository.GetByIdAsync(userId);
            if (!result.IsSuccess)
                return result;
            result.Data.Favorites++;
            return await userStatisticRepository.UpdateAsync(result.Data);
        }
        public async Task<Response<UserStatistic>> RemoveFavorite(int userId)
        {
            var result = await userStatisticRepository.GetByIdAsync(userId);
            if (!result.IsSuccess)
                return result;
            result.Data.Favorites--;
            return await userStatisticRepository.UpdateAsync(result.Data);
        }
        public async Task<Response<UserStatistic>> AddRating(int userId, float rating)
        {
            throw new NotImplementedException();
        }

    }
}
