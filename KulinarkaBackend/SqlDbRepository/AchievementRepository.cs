using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Kulinarka.SqlDbRepository
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly DbSet<Achievement> dbSet;
        private readonly IRepository<Achievement> repository;
        public AchievementRepository(AppDbContext context, IRepository<Achievement> repository)
        {
            this.dbSet = context.Achievements;
            this.repository = repository;
        }

        public async Task<Response<Achievement>> BeginTransactionAsync()
        {
           var result = await repository.BeginTransactionAsync();
            return result;
        }

        public async Task<Response<Achievement>> CommitTransactionAsync()
        {
            var result = await repository.CommitTransactionAsync();
            return result;
        }
        public async Task<Response<Achievement>> RollbackTransactionAsync()
        {
           var result = await repository.RollbackTransactionAsync();
            return result;
        }
        public async Task<Response<Achievement>> CreateAsync(Achievement entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity,saveChanges);
        }

        public async Task<Response<Achievement>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id,saveChanges);
        }

        public async Task<Response<List<Achievement>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<Achievement>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<Achievement>> UpdateAsync(int id, Achievement entity,bool saveChanges=true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
        public async Task<Response<Achievement>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }
    }
}
