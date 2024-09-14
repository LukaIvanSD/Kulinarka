using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly DbSet<Ingredient> dbSet;
        private readonly IRepository<Ingredient> repository;
        public IngredientRepository(AppDbContext context,IRepository<Ingredient> repository)
        {
            this.repository = repository;
            dbSet = context.Ingredients;
        }

        public async Task<Response<Ingredient>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<Ingredient>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<Ingredient>> CreateAsync(Ingredient entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<Ingredient>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<Ingredient>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<Ingredient>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<Ingredient>> GetByNameAsync(string ingredientName)
        {
            try 
            {
                var ingredient = await dbSet.FirstOrDefaultAsync(i => i.Name == ingredientName);
                return Response<Ingredient>.Success(ingredient, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<Ingredient>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<Ingredient>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<Ingredient>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<Ingredient>> UpdateAsync(int id, Ingredient entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
