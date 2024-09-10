using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IUserStatisticService
    {
        Task<Response<UserStatistic>> GetUserStatisticAsync(int userId);
        Task<Response<UserStatistic>> AddFollower(int userId);
        Task<Response<UserStatistic>> RemoveFollower(int userId);
        Task<Response<UserStatistic>> AddLike(int userId);
        Task<Response<UserStatistic>> RemoveLike(int userId);
        Task<Response<UserStatistic>> AddFavorite(int userId);
        Task<Response<UserStatistic>> RemoveFavorite(int userId);
        Task<Response<UserStatistic>> AddRating(int userId, float rating);
    }
}
