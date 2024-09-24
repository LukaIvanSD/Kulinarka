using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class UserAchievementRepository : IUserAchievementRepository
    {
        private readonly IRepository<UserAchievement> repository;
        private readonly DbSet<UserAchievement> dbSet;
        public UserAchievementRepository(AppDbContext dbContext, IRepository<UserAchievement> repository)
        {
            this.repository = repository;
            dbSet = dbContext.UserAchievement;
        }
        public async Task<Response<UserAchievement>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<UserAchievement>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<UserAchievement>> CreateAsync(UserAchievement entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<UserAchievement>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public Task<Response<List<UserAchievement>>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public async Task<Response<UserAchievement>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<UserAchievement>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<UserAchievement>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<UserAchievement>> UpdateAsync(int id, UserAchievement entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
        public async Task<Response<List<UserAchievement>>> GetUserAchievementsEagerAsync(int userId)
        {
            try
            {
                var userAchievements = await dbSet.Where(ua => ua.UserId == userId).Include(ua => ua.Achievement).ToListAsync();
                return Response<List<UserAchievement>>.Success(userAchievements, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<UserAchievement>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<UserAchievement>> UpdateUserAchievementAsync(UserAchievement userAchievement, bool saveChanges = true)
        {
            try
            {
                dbSet.Update(userAchievement);
                if (saveChanges)
                    await SaveChangesAsync();
                return Response<UserAchievement>.Success(userAchievement, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<UserAchievement>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<int>> GetCompletedAchievementsNumber(int userId)
        {
            try
            {
                var completedAchievementsNumber = await dbSet.Where(ua => ua.UserId == userId && ua.IsCompleted()).CountAsync();
                return Response<int>.Success(completedAchievementsNumber, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<int>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
