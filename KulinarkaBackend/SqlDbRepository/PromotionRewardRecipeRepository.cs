using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class PromotionRewardRecipeRepository : IPromotionRewardRecipeRepository
    {
        private readonly DbSet<PromotionRewardRecipe> dbSet;
        private readonly IRepository<PromotionRewardRecipe> repository;
        public PromotionRewardRecipeRepository(AppDbContext context,IRepository<PromotionRewardRecipe> repository)
        {
            dbSet = context.PromotionRewardsRecipe;
            this.repository = repository;
        }
        public async Task<Response<PromotionRewardRecipe>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<PromotionRewardRecipe>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<PromotionRewardRecipe>> CreateAsync(PromotionRewardRecipe entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<PromotionRewardRecipe>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }
        public async Task<Response<PromotionRewardRecipe>> DeleteAsync(PromotionRewardRecipe entity, bool saveChanges = true)
        {
            try
            {
                dbSet.Remove(entity);
                if (saveChanges)
                {
                    var result = await SaveChangesAsync();
                    if (!result.IsSuccess)
                        return Response<PromotionRewardRecipe>.Failure(result.ErrorMessage, result.StatusCode);
                }
                return Response<PromotionRewardRecipe>.Success(entity, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<PromotionRewardRecipe>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
        public async Task<Response<List<PromotionRewardRecipe>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<PromotionRewardRecipe>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<PromotionRewardRecipe>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<PromotionRewardRecipe>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<PromotionRewardRecipe>> UpdateAsync(int id, PromotionRewardRecipe entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
        public async Task<Response<PromotionRewardRecipe>> UpdateAsync(PromotionRewardRecipe entity, bool saveChanges = true)
        {
            try { 
                dbSet.Update(entity);
                if (saveChanges)
                {
                    var result = await SaveChangesAsync();
                    if (!result.IsSuccess)
                        return Response<PromotionRewardRecipe>.Failure(result.ErrorMessage, result.StatusCode);
                }
                return Response<PromotionRewardRecipe>.Success(entity, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<PromotionRewardRecipe>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
