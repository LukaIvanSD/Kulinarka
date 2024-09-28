using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class TagRepository : ITagRepository
    {
        private readonly DbSet<Tag> dbSet;
        private readonly IRepository<Tag> repository;
        public TagRepository(AppDbContext context, IRepository<Tag> repository)
        {
            this.repository = repository;
            dbSet = context.Tags;
        }

        public async Task<Response<Tag>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<Tag>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<Tag>> CreateAsync(Tag entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<Tag>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<Tag>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<Tag>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<Tag>> GetByTypeAndNameAsync(TagType preparationTime, string name)
        {
            try 
            { 
                Tag foungTag=await dbSet.FirstOrDefaultAsync(tag => tag.TagType == preparationTime && tag.Name == name);
                return Response<Tag>.Success(foungTag, StatusCode.OK);

            }
            catch (Exception e)
            {
                return Response<Tag>.Failure(e.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<Tag>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<Tag>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<Tag>> UpdateAsync(int id, Tag entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
