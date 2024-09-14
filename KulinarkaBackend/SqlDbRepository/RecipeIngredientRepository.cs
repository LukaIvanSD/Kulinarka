using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly DbSet<RecipeIngredient> dbSet;
        private readonly IRepository<RecipeIngredient> repository;
        public RecipeIngredientRepository(AppDbContext context,IRepository<RecipeIngredient> repository)
        {
            this.repository = repository;
            dbSet = context.RecipeIngredient;
        }

        public async Task<Response<RecipeIngredient>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<RecipeIngredient>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<RecipeIngredient>> CreateAsync(RecipeIngredient entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<RecipeIngredient>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<RecipeIngredient>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<RecipeIngredient>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<RecipeIngredient>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<RecipeIngredient>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<RecipeIngredient>> UpdateAsync(int id, RecipeIngredient entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
