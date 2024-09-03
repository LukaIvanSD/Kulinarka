using Kulinarka.Interfaces;
using Kulinarka.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext dbContext;
        public UserService(AppDbContext dbContext) { 
        this.dbContext = dbContext;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            User user = await dbContext.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<bool> EmailExistsAsync(string email)
        {
           return await dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await dbContext.Users.FindAsync(id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (!await IsUpdatedUserUnique(user))
                return false;
            try
            {
                dbContext.Entry(user).State = EntityState.Modified;

                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                Console.WriteLine($"Concurrency error: {dbEx.Message}");
                return false;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return false;
            }
        }

        private async Task<bool> IsUpdatedUserUnique(User user)
        {
            return !await dbContext.Users.AnyAsync(u => (u.Username == user.Username && u.Id != user.Id) || (u.Email == user.Email && user.Id != u.Id));
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await dbContext.Users.AnyAsync(u => u.Username == username);
        }
        public async Task<bool> IsUserUnique(User user)
        {
            return !await dbContext.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email);
        }

        public async Task<User>RegisterUserAsync(User user)
        {
            if (!await IsUserUnique(user))
                return null;
            try
            {
                user.DateOfCreation = DateTime.Now;
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update error: {dbEx.Message}");
                return null;
            }
        }
    }
}
