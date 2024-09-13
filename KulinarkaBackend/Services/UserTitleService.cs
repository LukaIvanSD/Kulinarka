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

        public async Task<Response<bool>> UpdateUserTitle(User user)
        {
            var userTitleResult = await GetUserTitleEagerAsync(user.Id);
            if (!userTitleResult.IsSuccess)
                return Response<bool>.Failure(userTitleResult.ErrorMessage, StatusCode.InternalServerError);
            user.UserTitle = userTitleResult.Data;
            var titleUpdateResult = await UpdateTitle(user);
            if (!titleUpdateResult.IsSuccess)
                return Response<bool>.Failure(titleUpdateResult.ErrorMessage, titleUpdateResult.StatusCode);
            return Response<bool>.Success(titleUpdateResult.Data, StatusCode.OK);
        }

        private async Task<Response<UserTitle>> SaveUserTitleToDb(User user)
        {
            var result = await userTitleRepository.UpdateAsync(user.UserTitle,false);
            if (!result.IsSuccess)
                return Response<UserTitle>.Failure(result.ErrorMessage, StatusCode.InternalServerError);
            return Response<UserTitle>.Success(result.Data, StatusCode.OK);

        }
        private async Task<Response<bool>> UpdateTitle(User user)
        {
            int achievements = user.UserAchievements.Where(ua => ua.IsCompleted()).Count();
            if (user.UserTitle.Demote(achievements))
                return Response<bool>.Success(true,StatusCode.OK);
            var titleResult = await titleService.GetNextTitle(user.UserTitle.TitleId);
            if(!titleResult.IsSuccess)
                return Response<bool>.Failure(titleResult.ErrorMessage, titleResult.StatusCode);
            user.UserTitle.NextTitle = titleResult.Data;
            if (user.UserTitle.NextTitle == null)
                return Response<bool>.Success(false, StatusCode.OK);
            if (user.UserTitle.Promote(achievements))
                return Response<bool>.Success(true, StatusCode.OK);
            return Response<bool>.Success(false, StatusCode.OK);
        }
    }
}
