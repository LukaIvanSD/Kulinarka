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

        public async Task<Response<List<Recipe>>> GetRecipesAndPromotionsAndOwnerEagerAsync()
        {
            try 
            { 
                List<Recipe>recipes= await dbSet.Include(r=>r.User).Include(r=>r.Promotions).ThenInclude(prr=>prr.PromotionReward).ToListAsync();
                return Response<List<Recipe>>.Success(recipes,StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<Recipe>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public  async Task<Response<List<Recipe>>> GetUserRecipesWithPromotionsEagerAsync(int userId)
        {
            try
            {
                List<Recipe> recipes = await dbSet.Where(u=>u.UserId==userId).Include(r => r.Promotions).ThenInclude(prr => prr.PromotionReward).ToListAsync();
                return Response<List<Recipe>>.Success(recipes, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<Recipe>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<Recipe>> GetRecipeDetailsEagerAsync(int id)
        {
            try { 
                Recipe foundRecipe = await dbSet
                    .Include(r=>r.Ingredients).ThenInclude(ri=>ri.Ingredient)
                    .Include(r=>r.Tags).ThenInclude(rt=>rt.Tag).AsSplitQuery()
                    .Include(r=>r.PreparationSteps).AsSplitQuery()
                    .Include(r => r.Promotions).ThenInclude(prr => prr.PromotionReward).AsSplitQuery()
                    .FirstOrDefaultAsync(r => r.Id == id);
                return Response<Recipe>.Success(foundRecipe, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<Recipe>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<int>> CountUserRecipes(int userId)
        {
            try
            {
                int count = dbSet.Count(r => r.UserId == userId);
                return Response<int>.Success(count, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<int>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
