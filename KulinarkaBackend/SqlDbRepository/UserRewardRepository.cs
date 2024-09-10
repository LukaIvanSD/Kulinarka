using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class UserRewardRepository : IUserRewardRepository
    {
        private readonly DbSet<UserReward> dbSet;
        private readonly IRepository<UserReward> repository;
        public UserRewardRepository(AppDbContext context,IRepository<UserReward> repository)
        {
            dbSet = context.UserRewards;
            this.repository = repository;
        }
        public async Task<Response<UserReward>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<UserReward>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<UserReward>> CreateAsync(UserReward entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<UserReward>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<UserReward>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<UserReward>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<UserReward>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<UserReward>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<UserReward>> UpdateAsync(int id, UserReward entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
