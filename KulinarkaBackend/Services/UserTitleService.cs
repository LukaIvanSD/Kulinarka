using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class UserTitleService : IUserTitleService
    {
        private readonly IUserTitleRepository userTitleRepository;
        private readonly ITitleService titleService; 
        public UserTitleService(IUserTitleRepository userTitleRepository,ITitleService titleService) {
            this.titleService = titleService;
            this.userTitleRepository = userTitleRepository;
        }

        public async Task<Response<UserTitle>> GetUserTitleEagerAsync(int userId)
        {
            return await userTitleRepository.GetUserTitleEagerAsync(userId);
        }

        public Task<Response<UserTitle>> GetUserTitleWithPromotionRewardEagerAsync(int userId)
        {
            return userTitleRepository.GetUserTitleWithPromotionRewardEagerAsync(userId);
        }

        public async Task<Response<UserTitle>> UpdateUserTitle(User user)
        {
            var userTitleResult = await GetUserTitleEagerAsync(user.Id);
            if (!userTitleResult.IsSuccess)
                return Response<UserTitle>.Failure(userTitleResult.ErrorMessage, StatusCode.InternalServerError);
            user.UserTitle = userTitleResult.Data;
            if (!UpdateTitle(user).Result)
                return Response<UserTitle>.Failure("Didnt update title", StatusCode.BadRequest);
            return await SaveUserTitleToDb(user);
        }

        private async Task<Response<UserTitle>> SaveUserTitleToDb(User user)
        {
            var result = await userTitleRepository.UpdateAsync(user.UserTitle);
            if (!result.IsSuccess)
                return Response<UserTitle>.Failure(result.ErrorMessage, StatusCode.InternalServerError);
            return Response<UserTitle>.Success(result.Data, StatusCode.OK);

        }
        private async Task<bool> UpdateTitle(User user)
        {
            int achievements = user.UserAchievements.Where(ua => ua.IsCompleted()).Count();
            if (user.UserTitle.Demote(achievements))
                return true;
            user.UserTitle.NextTitle = (await titleService.GetNextTitle(user.UserTitle.TitleId)).Data;
            if (user.UserTitle.NextTitle == null)
                return false;
            if (user.UserTitle.Promote(achievements))
                return true;
            return false;
        }
    }
}
