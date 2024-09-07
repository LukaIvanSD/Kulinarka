using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class UserRepository: IUserRepository
    {
        private readonly IRepository<User> repository;
        private readonly DbSet<User> dbSet;
        public UserRepository(AppDbContext dbContext,IRepository<User> repository)
        {
            this.repository = repository;
            dbSet = dbContext.Users;
        }

        public async Task<Response<User>> CreateAsync(User user, bool saveChanges = true)
        {
            return await repository.CreateAsync(user,saveChanges);
        }

        public async Task<Response<User>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id,saveChanges);
        }

        public async Task<Response<List<User>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<User>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<User>> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await dbSet.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                    return Response<User>.Failure("User not found", StatusCode.NotFound);
                return Response<User>.Success(user, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<User>.Failure("Error fetching data: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<User>> UpdateAsync(int id, User user, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, user);
        }
        public async Task<bool> IsUserUnique(User user)
        {
            return !await dbSet.AnyAsync(u => u.Username == user.Username || u.Email == user.Email);
        }

        public Task<Response<User>> BeginTransactionAsync()
        {
            return repository.BeginTransactionAsync();
        }

        public Task<Response<User>> CommitTransactionAsync()
        {
            return repository.CommitTransactionAsync();
        }

        public Task<Response<User>> RollbackTransactionAsync()
        {
            return repository.RollbackTransactionAsync();
        }

        public Task<Response<User>> SaveChangesAsync()
        {
            return repository.SaveChangesAsync();
        }
    }
}
