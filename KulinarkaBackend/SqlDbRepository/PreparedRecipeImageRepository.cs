using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class PreparedRecipeImageRepository : IPreparedRecipeImageRepository
    {
        private readonly DbSet<PreparedRecipeImage> dbSet;
        private readonly IRepository<PreparedRecipeImage> repository;
        public PreparedRecipeImageRepository(AppDbContext context, IRepository<PreparedRecipeImage> repository)
        {
            dbSet = context.PreparedRecipeImages;
            this.repository = repository;
        }
        public async Task<Response<PreparedRecipeImage>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<PreparedRecipeImage>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<PreparedRecipeImage>> CreateAsync(PreparedRecipeImage entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<PreparedRecipeImage>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<PreparedRecipeImage>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<PreparedRecipeImage>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<List<PreparedRecipeImage>>> GetByRecipeIdPagedAsync(int recipeId,int startIndex, int resultCount)
        {
            try 
            { 
                List<PreparedRecipeImage> preparedRecipeImages = await dbSet
                    .Where(p => p.RecipeId == recipeId)
                    .OrderByDescending(pri=>pri.DateUploaded)
                    .Skip(startIndex)
                    .Take(resultCount)
                    .ToListAsync();
                return Response<List<PreparedRecipeImage>>.Success(preparedRecipeImages,StatusCode.OK);
            }
            catch(Exception ex)
            {
                return Response<List<PreparedRecipeImage>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<PreparedRecipeImage>>> GetByRecipeIdPagedWithCreatorEagerAsync(int id, int pageNumber, int pageSize)
        {
            try
            {
                List<PreparedRecipeImage> preparedRecipeImages = await dbSet
                    .Where(p => p.RecipeId == id)
                    .Include(p => p.Creator)
                    .OrderByDescending(pri => pri.DateUploaded)
                    .Skip(pageNumber)
                    .Take(pageSize)
                    .ToListAsync();
                return Response<List<PreparedRecipeImage>>.Success(preparedRecipeImages, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<PreparedRecipeImage>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<PreparedRecipeImage>>> GetByUserAndRecipeIdAsync(int userId, int recipeId)
        {
            try
            {
                List<PreparedRecipeImage> preparedRecipeImages = await dbSet
                    .Where(p => p.CreatorId == userId && p.RecipeId == recipeId)
                    .ToListAsync();
                return Response<List<PreparedRecipeImage>>.Success(preparedRecipeImages, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<PreparedRecipeImage>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<PreparedRecipeImage>>> GetByUserIdPagedAsync(int userId,int startIndex,int resultCount)
        {
            try
            {
                List<PreparedRecipeImage> preparedRecipeImages = await dbSet
                    .Where(p => p.CreatorId == userId)
                    .OrderBy(pri => pri.DateUploaded)
                    .Skip(startIndex)
                    .Take(resultCount)
                    .ToListAsync();
                return Response<List<PreparedRecipeImage>>.Success(preparedRecipeImages, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<PreparedRecipeImage>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<PreparedRecipeImage>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<PreparedRecipeImage>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<PreparedRecipeImage>> UpdateAsync(int id, PreparedRecipeImage entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
