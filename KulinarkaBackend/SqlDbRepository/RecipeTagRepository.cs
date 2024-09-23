using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class RecipeTagRepository : IRecipeTagRepository
    {
        private readonly DbSet<RecipeTag> dbSet;
        private readonly IRepository<RecipeTag> repository;
        public RecipeTagRepository(AppDbContext context,IRepository<RecipeTag> repository)
        {
            dbSet = context.RecipeTag;
            this.repository = repository;
        }

        public async Task<Response<RecipeTag>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<RecipeTag>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<RecipeTag>> CreateAsync(RecipeTag entity, bool saveChanges = true)
        { 
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<RecipeTag>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<RecipeTag>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<RecipeTag>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<List<RecipeTag>>> GetByRecipeIdEagerAsync(int recipeId)
        {
            try
            {
                List<RecipeTag> recipeTags = await dbSet.Where(rt=>rt.RecipeId==recipeId).Include(rt=>rt.Tag).ToListAsync();
                return Response<List<RecipeTag>>.Success(recipeTags,StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<RecipeTag>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<RecipeTag>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<RecipeTag>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<RecipeTag>> UpdateAsync(int id, RecipeTag entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
