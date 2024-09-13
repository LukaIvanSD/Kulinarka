using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace Kulinarka.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserTitleService userTitleService;
        private readonly IUserService userService;
        private readonly IPromotionRewardRecipeService promotionRewardRecipeService;
        public UserManagementService(IUserTitleService userTitleService,IUserService userService,IPromotionRewardRecipeService promotionRewardRecipeService)
        {
            this.userTitleService = userTitleService;
            this.userService = userService;
            this.promotionRewardRecipeService = promotionRewardRecipeService;
        }
        public async Task<Response<User>> AddProgress(int userId, RequirementType requirementType)
        {
            var userResult = await userService.GetUserAchievementsEagerAsync(userId);
            if (!userResult.IsSuccess)
                return Response<User>.Failure(userResult.ErrorMessage, StatusCode.InternalServerError);
            User user= userResult.Data;

            var beginTransaction = await userService.BeginTransactionAsync();
            if (!beginTransaction.IsSuccess)
                return Response<User>.Failure(beginTransaction.ErrorMessage, StatusCode.InternalServerError);
            
            try
            {
                int achievementsJustCompleted = user.AddPoint(requirementType);
                var userTitleUpdateResult = await UpdateUserTitle(achievementsJustCompleted, user);
                if (!userTitleUpdateResult.IsSuccess)
                    throw new Exception(userTitleUpdateResult.ErrorMessage);
                var promotionUpdateResult = await UpdateUserPromotions(userTitleUpdateResult.Data,user);
                if (!promotionUpdateResult.IsSuccess)
                    throw new Exception(promotionUpdateResult.ErrorMessage);
                var result = await userService.SaveChangesAsync();
                if (!result.IsSuccess)
                   throw new Exception(result.ErrorMessage);
                result = await userService.CommitTransactionAsync();
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                await userService.RollbackTransactionAsync();
                return Response<User>.Failure(ex.Message, StatusCode.InternalServerError);
            }
            return Response<User>.Success(user, StatusCode.OK);
        }
        public async Task<Response<User>> RemoveProgress(int userId, RequirementType requirementType)
        {
            var userResult = await userService.GetUserAchievementsEagerAsync(userId);
            if (!userResult.IsSuccess)
                return Response<User>.Failure(userResult.ErrorMessage, StatusCode.InternalServerError);
            User user = userResult.Data;

            var beginTransaction = await userService.BeginTransactionAsync();
            if (!beginTransaction.IsSuccess)
                return Response<User>.Failure(beginTransaction.ErrorMessage, StatusCode.InternalServerError);
            try
            {
                int achievementsJustRevoked = user.RemovePoint(requirementType);
                var userTitleUpdateResult = await UpdateUserTitle(achievementsJustRevoked, user);
                if (!userTitleUpdateResult.IsSuccess)
                    throw new Exception(userTitleUpdateResult.ErrorMessage);
                var promotionUpdateResult = await UpdateUserPromotions(userTitleUpdateResult.Data,user);
                if (!promotionUpdateResult.IsSuccess)
                    throw new Exception(promotionUpdateResult.ErrorMessage);
                var result = await userService.SaveChangesAsync();
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                result = await userService.CommitTransactionAsync();
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                return Response<User>.Success(user, StatusCode.OK);
            }
            catch (Exception ex)
            {
                await userService.RollbackTransactionAsync();
                return Response<User>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
        private async Task<Response<bool>> UpdateUserTitle(int achievementsChanged, User user)
        {
            if (achievementsChanged == 0)
                return Response<bool>.Success(false, StatusCode.OK);
            var userTitleResult = await userTitleService.UpdateUserTitle(user);
            return userTitleResult;
        }

        private async Task<Response<PromotionRewardRecipe>> UpdateUserPromotions(bool IsTitleUpdated,User user)
        {
            if (!IsTitleUpdated)
                return Response<PromotionRewardRecipe>.Success(null, StatusCode.OK);
            var result = await promotionRewardRecipeService.UpdateUserPromotions(user,user.UserTitle.TitleId);
            return result;
        }
    }
}
