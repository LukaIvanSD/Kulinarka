using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class RecipeStatisticsRepostiory : IRecipeStatisticsRepository
    {
        private readonly DbSet<RecipeStatistics> dbSet;
        private readonly IRepository<RecipeStatistics> repository;
        public RecipeStatisticsRepostiory(AppDbContext context,IRepository<RecipeStatistics> repository)
        {
            dbSet = context.RecipeStatistics;
            this.repository = repository;
        }
        public async Task<Response<RecipeStatistics>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<RecipeStatistics>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<RecipeStatistics>> CreateAsync(RecipeStatistics entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<RecipeStatistics>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<RecipeStatistics>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<RecipeStatistics>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<RecipeStatistics>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<RecipeStatistics>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<RecipeStatistics>> UpdateAsync(int id, RecipeStatistics entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
