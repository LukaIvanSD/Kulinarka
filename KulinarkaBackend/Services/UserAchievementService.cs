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
        private readonly IUserService userService;
        private readonly IUserTitleService userTitleService;
        public UserAchievementService(IUserAchievementRepository userAchievementRepository, IUserService userService,IUserTitleService userTitleService)
        {
            this.userAchievementRepository = userAchievementRepository;
            this.userService = userService;
            this.userTitleService = userTitleService;
        }

        public async Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId)
        {
            return await userAchievementRepository.GetUserAchievementsEagerAsync(userId);
        }
        public async Task<Response<List<UserAchievement>>> AddProgress(int userId,RequirementType requirementType)
        {
            var userResult = await userService.GetUserAchievementsEagerAsync(userId);
            if (!userResult.IsSuccess)
                return Response<List<UserAchievement>>.Failure(userResult.ErrorMessage, StatusCode.InternalServerError);
            int achievementsjustCompleted=userResult.Data.AddPoint(requirementType);
            if (achievementsjustCompleted != 0) 
               await userTitleService.UpdateUserTitle(userResult.Data);
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
            var userResult = await userService.GetUserAchievementsEagerAsync(userId);
            if (!userResult.IsSuccess)
                return Response<List<UserAchievement>>.Failure(userResult.ErrorMessage, StatusCode.InternalServerError);
            int achievementsJustRevoked = userResult.Data.RemovePoint(requirementType);
            if (achievementsJustRevoked != 0)
                await userTitleService.UpdateUserTitle(userResult.Data);
            return await SaveUserAchievementsToDb(userResult.Data.UserAchievements);
        }
    }
}
