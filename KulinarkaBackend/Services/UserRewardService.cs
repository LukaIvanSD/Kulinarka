using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class UserRewardService : IUserRewardService
    {
        private readonly IUserRewardRepository userRewardRepository;
        public UserRewardService(IUserRewardRepository userRewardRepository) {
        this.userRewardRepository = userRewardRepository;
        }
    }
}
