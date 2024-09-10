using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class UserStatisticRepository : IUserStatisticRepository
    {
        private readonly IRepository<UserStatistic> repository;
        private readonly DbSet<UserStatistic> dbSet;
        public UserStatisticRepository(AppDbContext context,IRepository<UserStatistic> repository)
        {
            dbSet = context.UserStatistics;
            this.repository = repository;
        }

        public async Task<Response<UserStatistic>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<UserStatistic>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<UserStatistic>> CreateAsync(UserStatistic entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<UserStatistic>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<UserStatistic>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<UserStatistic>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<UserStatistic>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<UserStatistic>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<UserStatistic>> UpdateAsync(int id, UserStatistic entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
        public async Task<Response<UserStatistic>> UpdateAsync(UserStatistic entity, bool saveChanges = true)
        {
            try 
            { 
                dbSet.Update(entity);
                if (saveChanges)
                    await SaveChangesAsync();
                return Response<UserStatistic>.Success(entity, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<UserStatistic>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
