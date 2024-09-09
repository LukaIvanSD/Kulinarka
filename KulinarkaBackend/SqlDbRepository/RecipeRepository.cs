using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DbSet<Recipe> dbSet;
        private readonly IRepository<Recipe> repository;
        public RecipeRepository(AppDbContext context,IRepository<Recipe> repository)
        {
            this.dbSet = context.Recipes;
            this.repository = repository;
        }
        public async Task<Response<Recipe>> CreateAsync(Recipe recipe, bool saveChanges = true)
        {
            return await repository.CreateAsync(recipe,saveChanges);
        }

        public async Task<Response<Recipe>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id,saveChanges);
        }

        public async Task<Response<List<Recipe>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<Recipe>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<Recipe>> UpdateAsync(int id, Recipe entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity,saveChanges);
        }

        public async Task<Response<Recipe>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public Task<Response<Recipe>> BeginTransactionAsync()
        {
            return repository.BeginTransactionAsync();
        }

        public Task<Response<Recipe>> CommitTransactionAsync()
        {
            return repository.CommitTransactionAsync();
        }

        public Task<Response<Recipe>> RollbackTransactionAsync()
        {
            return repository.RollbackTransactionAsync();
        }
    }
}
