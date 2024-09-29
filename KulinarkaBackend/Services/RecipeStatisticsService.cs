using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class RecipeStatisticsService : IRecipeStatisticsService
    {
        private readonly IRecipeStatisticsRepository recipeStatisticsRepository;
        public RecipeStatisticsService(IRecipeStatisticsRepository recipeStatisticsRepository)
        {
            this.recipeStatisticsRepository = recipeStatisticsRepository;
        }
        public async Task<Response<RecipeStatistics>> AddAsync(int recipeId,bool saveChanges=true)
        {
            RecipeStatistics recipeStatistics = CreateRecipeStatistics(recipeId);
            var result = await recipeStatisticsRepository.CreateAsync(recipeStatistics,saveChanges);
            if (!result.IsSuccess)
                return Response<RecipeStatistics>.Failure(result.ErrorMessage, result.StatusCode);
            return Response<RecipeStatistics>.Success(result.Data,StatusCode.OK);
        }

        private static RecipeStatistics CreateRecipeStatistics(int recipeId)
        {
            return new RecipeStatistics
            {
                RecipeId = recipeId,
                Likes = 0,
                Shares = 0,
                Views = 0,
                Favorites = 0,
                Comments = 0,
                AverageRating = 0
            };
        }

        public async Task<Response<RecipeStatistics>> AddCommentStatisticAsync(int recipeId, bool saveChanges = true)
        {
            var result = await recipeStatisticsRepository.GetByIdAsync(recipeId);
            if (!result.IsSuccess)
                return Response<RecipeStatistics>.Failure(result.ErrorMessage, result.StatusCode);
            RecipeStatistics recipeStatistics = result.Data;
            recipeStatistics.AddComment();
            return await recipeStatisticsRepository.UpdateAsync(recipeStatistics.RecipeId,recipeStatistics, saveChanges);
        }
    }
}
