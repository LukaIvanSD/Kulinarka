using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class PromotionRewardRepository : IPromotionRewardRepository
    {
        private readonly IRepository<PromotionReward> repository;
        private readonly DbSet<PromotionReward> dbSet;
        public PromotionRewardRepository(AppDbContext context,IRepository<PromotionReward> repository)
        {
            this.repository = repository;
            dbSet = context.PromotionRewards;
        }
        public async Task<Response<PromotionReward>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<PromotionReward>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<PromotionReward>> CreateAsync(PromotionReward entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<PromotionReward>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<PromotionReward>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<PromotionReward>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<PromotionReward>> GetByTitleId(int titleId)
        {
            try { 
                PromotionReward promotionReward = await dbSet.FirstOrDefaultAsync(x => x.TitleId == titleId);
                if (promotionReward == null)
                    return Response<PromotionReward>.Failure("Promotion reward not found", StatusCode.NotFound);
                return Response<PromotionReward>.Success(promotionReward,StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<PromotionReward>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<PromotionReward>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<PromotionReward>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<PromotionReward>> UpdateAsync(int id, PromotionReward entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }

        public Task<Response<PromotionReward>> UpdateAsync(PromotionReward entity, bool saveChanges = true)
        {
            throw new NotImplementedException();
        }
    }
}
