using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMapper mapper;
        private IUserService userService;
        private ITitleService titleService;
        private IRecipeService recipeService;
        private IUserAchievementService userAchievementService;
        public ProfileService(IMapper mapper,IUserService userService,IRecipeService recipeService,IUserAchievementService userAchievementService,ITitleService titleService)
        {
            this.userAchievementService = userAchievementService; 
            this.recipeService = recipeService;
            this.userService = userService;
            this.mapper = mapper;
            this.titleService = titleService;
        }
        public async Task<Response<ProfileDTO>> GetProfileDTO(int userId)
        {
            var userResult =await userService.GetUserTitleAndStatisticAndRewardsEagerAsync(userId);
            if (!userResult.IsSuccess)
                return Response<ProfileDTO>.Failure(userResult.ErrorMessage, userResult.StatusCode);
            User user = userResult.Data;
            var nextTitleResult= await titleService.GetNextTitleAndRewardsEagerAsync(user.UserTitle.TitleId);
            if (!nextTitleResult.IsSuccess)
                return Response<ProfileDTO>.Failure(nextTitleResult.ErrorMessage, nextTitleResult.StatusCode);
            user.UserTitle.NextTitle = nextTitleResult.Data;
            var recipeCountResult = await recipeService.CountUserRecipes(user.Id);
            var achievementsCompletedResult = await userAchievementService.GetCompletedAchievementsNumber(user.Id);

            UserInfoDTO userInfoDTO = mapper.Map<UserInfoDTO>(user);
            UserStatisticDTO userStatisticDTO = mapper.Map<UserStatisticDTO>(user, opt => {
                opt.Items["RecipeCount"] =recipeCountResult.Data;
            });
            TitleDTO titleDTO = mapper.Map<TitleDTO>(user, opt => {
                opt.Items["AchievementsCompleted"] =achievementsCompletedResult.Data;
            });
            PromotionRewardDTO currentReward =mapper.Map<PromotionRewardDTO>(user.GetTitleRewards());
            PromotionRewardDTO nextReward = mapper.Map<PromotionRewardDTO>(user.GetNextTitleRewards());

            ProfileDTO profileDTO = new ProfileDTO(userInfoDTO,titleDTO, userStatisticDTO,currentReward,nextReward);
            return  Response<ProfileDTO>.Success(profileDTO,StatusCode.OK);
        }
    }
}
