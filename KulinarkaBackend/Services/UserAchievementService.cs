using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class UserAchievementService : IUserAchievementService
    {
        private readonly IUserAchievementRepository userAchievementRepository;

        private readonly Func<IUserService> userServiceFactory;
        private readonly Func<IAchievementService> achievementServiceFactory;
        public UserAchievementService(IUserAchievementRepository userAchievementRepository, Func<IUserService> userServiceFactory, Func<IAchievementService> achievementServiceFactory)
        {
            this.userAchievementRepository = userAchievementRepository;
            this.userServiceFactory = userServiceFactory;
            this.achievementServiceFactory = achievementServiceFactory;
        }

        public async Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId)
        {
            return await userAchievementRepository.GetUserAchievementsEagerAsync(userId);
        }

        private async Task<Response<List<UserAchievement>>> SaveUserAchievementsToDb(ICollection<UserAchievement> userAchievements)
        {
                foreach (var userAchievement in userAchievements)
                {
                    var result = await userAchievementRepository.UpdateUserAchievementAsync(userAchievement,false);
                    if (!result.IsSuccess)
                        throw new Exception(result.ErrorMessage);
                }

            return Response<List<UserAchievement>>.Success(userAchievements.ToList(), StatusCode.OK);
        }
        public async Task<Response<Achievement>> CreateUsersAchievement(Achievement achievement)
        {
            IUserService userService = userServiceFactory();
            var result = await userService.GetUsersAsync();
            if (!result.IsSuccess)
                return Response<Achievement>.Failure("Error fetching users", StatusCode.InternalServerError);
            achievement.UserAchievements = new List<UserAchievement>();
            foreach (User user in result.Data)
            {
                UserAchievement usersAchievement = new UserAchievement(achievement.Id, user.Id);
                achievement.UserAchievements.Add(usersAchievement);
            }
            return Response<Achievement>.Success(achievement, StatusCode.OK);
        }

        public async Task<Response<List<UserAchievement>>> CreateUserAchievements(User user)
        {
            IAchievementService achievementService = achievementServiceFactory();
            List<UserAchievement> userAchievements = new List<UserAchievement>();
            var result = await achievementService.GetAchievementsAsync();
            if (!result.IsSuccess)
                return Response<List<UserAchievement>>.Failure(result.ErrorMessage, StatusCode.InternalServerError);
            foreach (Achievement achievement in result.Data)
            {
                UserAchievement userAchievement = new UserAchievement(achievement.Id, user.Id);
                userAchievements.Add(userAchievement);
            }
            return Response<List<UserAchievement>>.Success(userAchievements, StatusCode.OK);
        }

        public async Task<Response<int>> GetCompletedAchievementsNumber(int userId)
        {
            return await userAchievementRepository.GetCompletedAchievementsNumber(userId);
        }
    }
}
