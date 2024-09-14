using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class PreparationStepRepository : IPreparationStepRepository
    {
        private readonly DbSet<PreparationStep> dbSet;
        private readonly IRepository<PreparationStep> repository;
        public PreparationStepRepository(AppDbContext context, IRepository<PreparationStep> repository)
        {
            dbSet = context.PreparationStep;
            this.repository = repository;
        }
        public async Task<Response<PreparationStep>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<PreparationStep>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<PreparationStep>> CreateAsync(PreparationStep entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<PreparationStep>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<PreparationStep>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<PreparationStep>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<PreparationStep>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<PreparationStep>> SaveChangesAsync()
        {
           return await repository.SaveChangesAsync();
        }

        public async Task<Response<PreparationStep>> UpdateAsync(int id, PreparationStep entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
