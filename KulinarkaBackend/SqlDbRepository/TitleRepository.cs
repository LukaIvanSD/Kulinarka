using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class TitleRepository : ITitleRepository
    {
        private readonly DbSet<Title> dbSet;
        private readonly IRepository<Title> repository;
        public TitleRepository(AppDbContext context,IRepository<Title> repository)
        {
            this.dbSet = context.Titles;
            this.repository = repository;

        }
        public async Task<Response<Title>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<Title>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<Title>> CreateAsync(Title entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<Title>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<Title>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<Title>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public DbSet<Title> GetDbSet()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<Title>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<Title>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<Title>> UpdateAsync(int id, Title entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
