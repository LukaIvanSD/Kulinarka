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
        private readonly IUserTitleService userTitleService;
        public UserAchievementService(IUserAchievementRepository userAchievementRepository, Func<IUserService> userServiceFactory, IUserTitleService userTitleService, Func<IAchievementService> achievementServiceFactory)
        {
            this.userAchievementRepository = userAchievementRepository;
            this.userServiceFactory = userServiceFactory;
            this.userTitleService = userTitleService;
            this.achievementServiceFactory = achievementServiceFactory;
        }

        public async Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId)
        {
            return await userAchievementRepository.GetUserAchievementsEagerAsync(userId);
        }
        public async Task<Response<List<UserAchievement>>> AddProgress(int userId,RequirementType requirementType)
        {
            IUserService userService = userServiceFactory();
            var userResult = await userService.GetUserAchievementsEagerAsync(userId);
            if (!userResult.IsSuccess)
                return Response<List<UserAchievement>>.Failure(userResult.ErrorMessage, StatusCode.InternalServerError);
            int achievementsjustCompleted=userResult.Data.AddPoint(requirementType);
            if (achievementsjustCompleted != 0)
            {
                var result = await userTitleService.UpdateUserTitle(userResult.Data);
                if (!result.IsSuccess)
                    return Response<List<UserAchievement>>.Failure(result.ErrorMessage, StatusCode.InternalServerError);
            }
            return  await SaveUserAchievementsToDb(userResult.Data.UserAchievements);
        }

        private async Task<Response<List<UserAchievement>>> SaveUserAchievementsToDb(ICollection<UserAchievement> userAchievements)
        {
            var transactionResult = await userAchievementRepository.BeginTransactionAsync();
            if (!transactionResult.IsSuccess)
                return Response<List<UserAchievement>>.Failure(transactionResult.ErrorMessage, StatusCode.InternalServerError);

            try
            {
                foreach (var userAchievement in userAchievements)
                {
                    var result = await userAchievementRepository.UpdateUserAchievementAsync(userAchievement,false);
                    if (!result.IsSuccess)
                        throw new Exception(result.ErrorMessage);
                }
                transactionResult = await userAchievementRepository.SaveChangesAsync();
                if (!transactionResult.IsSuccess)
                    throw new Exception(transactionResult.ErrorMessage);
                transactionResult = await userAchievementRepository.CommitTransactionAsync();
                if (!transactionResult.IsSuccess)
                    throw new Exception(transactionResult.ErrorMessage);
            }
            catch (Exception ex)
            {
                await userAchievementRepository.RollbackTransactionAsync();
                return Response<List<UserAchievement>>.Failure(ex.Message, StatusCode.InternalServerError);

            }
            return Response<List<UserAchievement>>.Success(userAchievements.ToList(), StatusCode.OK);
        }

        public async Task<Response<List<UserAchievement>>> RemoveProgress(int userId, RequirementType requirementType)
        {
            IUserService userService = userServiceFactory();
            var userResult = await userService.GetUserAchievementsEagerAsync(userId);
            if (!userResult.IsSuccess)
                return Response<List<UserAchievement>>.Failure(userResult.ErrorMessage, StatusCode.InternalServerError);
            int achievementsJustRevoked = userResult.Data.RemovePoint(requirementType);
            if (achievementsJustRevoked != 0)
            {
                var result = await userTitleService.UpdateUserTitle(userResult.Data);
                if (!result.IsSuccess)
                    return Response<List<UserAchievement>>.Failure(result.ErrorMessage, StatusCode.InternalServerError);
            }
            return await SaveUserAchievementsToDb(userResult.Data.UserAchievements);
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
    }
}
