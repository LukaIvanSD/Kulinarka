using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository achievementRepository;
        private readonly IUserAchievementService userAchievementServiceFactory;
        public AchievementService(IAchievementRepository achievementRepository,IUserAchievementService userAchievementServiceFactory)
        {
            this.userAchievementServiceFactory = userAchievementServiceFactory;
            this.achievementRepository = achievementRepository;
        }
        public async Task<Response<Achievement>> AddAchievementAsync(Achievement achievement,bool saveChanges=true)
        {
            return await achievementRepository.CreateAsync(achievement,saveChanges);
        }
        public async Task<Response<Achievement>> DeleteAchievementAsync(int id, bool saveChanges = true)
        {
            return await achievementRepository.DeleteAsync(id,saveChanges);
        }
        public async Task<Response<List<Achievement>>> GetAchievementsAsync()
        {
            return await achievementRepository.GetAllAsync();
        }
        public async Task<Response<Achievement>> GetAchievementAsync(int id)
        {
            return await achievementRepository.GetByIdAsync(id);
        }
        public async Task<Response<Achievement>> UpdateAchievementAsync(Achievement achievement, bool saveChanges = true)
        {
            return await achievementRepository.UpdateAsync(achievement.Id, achievement,saveChanges);
        }

        public async Task<Response<Achievement>> CreateAchievement(Achievement achievement)
        {

            var result = await achievementRepository.BeginTransactionAsync();
            if (!result.IsSuccess)
                return result;
            try 
            {
                result = await AddAchievementAsync(achievement,false);
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                result = await userAchievementServiceFactory.CreateUsersAchievement(result.Data);
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                result = await achievementRepository.SaveChangesAsync();
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                result = await achievementRepository.CommitTransactionAsync();
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                return Response<Achievement>.Success(result.Data, StatusCode.OK);
            }
            catch (Exception ex)
            {
                await achievementRepository.RollbackTransactionAsync();
                return Response<Achievement>.Failure("Error creating achievement: " + ex.Message, StatusCode.InternalServerError);
            }

        }

    }
}
