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

        public Task<Response<User>> CreateAsync(User user)
        {
            return repository.CreateAsync(user);
        }

        public Task<Response<User>> DeleteAsync(int id)
        {
            return repository.DeleteAsync(id);
        }

        public Task<Response<List<User>>> GetAllAsync()
        {
            return repository.GetAllAsync();
        }

        public Task<Response<User>> GetByIdAsync(int id)
        {
            return repository.GetByIdAsync(id);
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

        public Task<Response<User>> UpdateAsync(int id, User user)
        {
            return repository.UpdateAsync(id, user);
        }
        public async Task<bool> IsUserUnique(User user)
        {
            return !await dbSet.AnyAsync(u => u.Username == user.Username || u.Email == user.Email);
        }

    }
}
