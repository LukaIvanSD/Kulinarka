using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IUserAchievementService userAchievementServiceFactory;

        public UserService(IUserRepository userRepository, IUserAchievementService userAchievementServiceFactory)
        {
            this.userRepository = userRepository;
            this.userAchievementServiceFactory = userAchievementServiceFactory;
        }
        public async Task<Response<User>> DeleteUserAsync(int id)
        {
            return await userRepository.DeleteAsync(id);
        }
        public async Task<Response<User>> GetUserByIdAsync(int id)
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
            var result=await InitializeUserAsync(user);
            if (!result.IsSuccess)
                return result;
            return await userRepository.CreateAsync(user);
        }

        private async Task<Response<User>> InitializeUserAsync(User user)
        {
            user.DateOfCreation = DateTime.Now;
            user.UserTitle= new UserTitle(user.Id);
            user.UserStatistic = new UserStatistic(user.Id);
            var result = await userAchievementServiceFactory.CreateUserAchievements(user);
            if (!result.IsSuccess)
                return Response<User>.Failure(result.ErrorMessage, StatusCode.InternalServerError);
            user.UserAchievements = result.Data;
            return Response<User>.Success(user, StatusCode.OK);
        }

        public async Task<Response<User>> GetUserAchievementsEagerAsync(int id)
        {
            return await userRepository.GetUserAchievementsEagerAsync(id);
        }

        public Task<Response<User>> GetByUsernameAsync(string username)
        {
            return userRepository.GetByUsernameAsync(username);
        }

        public Task<Response<User>> BeginTransactionAsync()
        {
            return userRepository.BeginTransactionAsync();
        }
        public async Task<Response<User>> CommitTransactionAsync()
        {
            return await userRepository.CommitTransactionAsync();
        }
        public async Task<Response<User>> RollbackTransactionAsync()
        {
            return await userRepository.RollbackTransactionAsync();
        }
        public async Task<Response<User>> SaveChangesAsync()
        {
            return await userRepository.SaveChangesAsync();
        }

        public Task<Response<User>> UpdateAsync(User user, bool saveChanges = true)
        {
            return userRepository.UpdateAsync(user.Id,user, saveChanges);
        }

        public Task<Response<User>> GetUserTitleAndStatisticAndRewardsEagerAsync(int userId)
        {
            return userRepository.GetUserTitleAndStatisticAndRewardsEagerAsync(userId);
        }
    }
}
