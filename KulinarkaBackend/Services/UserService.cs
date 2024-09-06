using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository) {
            this.userRepository = userRepository;
        }
        public async Task<Response<User>> DeleteUserAsync(int id)
        {
            return await userRepository.DeleteAsync(id);
        }
        public async Task<Response<User>> GetUserAsync(int id)
        {
            return await userRepository.GetByIdAsync(id);
        }

        public async Task<Response<List<User>>> GetUsersAsync()
        {
            return await userRepository.GetAllAsync();
        }

        public async Task<Response<User>> UpdateUserAsync(User loggedInUser,User user)
        {
            if(loggedInUser.Id!=user.Id)
                return Response<User>.Failure("You are not authorized to update this user", StatusCode.Unauthorized);
            if (!await userRepository.IsUserUnique(user))
                return Response<User>.Failure("Username or email already exists", StatusCode.BadRequest);    
            return await userRepository.UpdateAsync(user.Id, user);

        }

        public async Task<Response<User>>RegisterUserAsync(User user)
        {
            if (!await userRepository.IsUserUnique(user))
                return Response<User>.Failure("Username or email already exists", StatusCode.BadRequest);
                user.DateOfCreation = DateTime.Now;
                return await userRepository.CreateAsync(user);
        }
    }
}
