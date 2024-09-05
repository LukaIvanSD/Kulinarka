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
        public Task<Response<Recipe>> CreateAsync(Recipe recipe)
        {
            return repository.CreateAsync(recipe);
        }

        public Task<Response<Recipe>> DeleteAsync(int id)
        {
            return repository.DeleteAsync(id);
        }

        public Task<Response<List<Recipe>>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task<Response<Recipe>> GetByIdAsync(int id)
        {
            return repository.GetByIdAsync(id);
        }

        public Task<Response<Recipe>> UpdateAsync(int id, Recipe entity)
        {
            return repository.UpdateAsync(id, entity);
        }
    }
}
