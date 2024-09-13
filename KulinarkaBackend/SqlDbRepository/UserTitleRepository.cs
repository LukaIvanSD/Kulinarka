using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kulinarka.SqlDbRepository
{
    public class UserTitleRepository : IUserTitleRepository
    {
        private DbSet<UserTitle> dbSet;
        private readonly IRepository<UserTitle> repository;
        public UserTitleRepository(AppDbContext context, IRepository<UserTitle> repository)
        {
            dbSet = context.UserTitle;
            this.repository = repository;
        }

        public async Task<Response<UserTitle>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<UserTitle>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<UserTitle>> CreateAsync(UserTitle entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<UserTitle>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<UserTitle>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<UserTitle>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<UserTitle>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<UserTitle>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<UserTitle>> UpdateAsync(int id, UserTitle entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
        public async Task<Response<UserTitle>> GetUserTitleEagerAsync(int userId)
        {
            try
            {
                var userTitle = await dbSet.Include(ut => ut.CurrentTitle).FirstOrDefaultAsync(ut => ut.UserId == userId);
                if (userTitle == null)
                    return Response<UserTitle>.Failure("User title not found", StatusCode.NotFound);
                return Response<UserTitle>.Success(userTitle, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<UserTitle>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
        public async Task<Response<UserTitle>> UpdateAsync(UserTitle userTitle,bool saveChanges=true)
        {
            try
            {
                dbSet.Update(userTitle);
                if (saveChanges)
                {
                    var result = await SaveChangesAsync();
                    if (!result.IsSuccess)
                        return Response<UserTitle>.Failure(result.ErrorMessage, StatusCode.InternalServerError);
                }
                return Response<UserTitle>.Success(userTitle, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<UserTitle>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<UserTitle>> GetUserTitleWithPromotionRewardEagerAsync(int userId)
        {
            try
            {
                UserTitle userTitle = await dbSet.Include(ut => ut.CurrentTitle).ThenInclude(t => t.PromotionReward).FirstOrDefaultAsync(ut => ut.UserId == userId);
                if (userTitle == null)
                    return Response<UserTitle>.Failure("User title not found", StatusCode.NotFound);
                return Response<UserTitle>.Success(userTitle, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<UserTitle>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
