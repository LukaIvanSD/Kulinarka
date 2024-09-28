using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using System.Net;
namespace Kulinarka.Services
{
    public class PreparedRecipeImageService : IPreparedRecipeImageService
    {
        private readonly IPreparedRecipeImageRepository preparedRecipeImageRepository;
        private readonly IRecipeRepository recipeRepository;
        public PreparedRecipeImageService(IPreparedRecipeImageRepository preparedRecipeImageRepository,IRecipeRepository recipeRepository) {
            this.preparedRecipeImageRepository = preparedRecipeImageRepository;
            this.recipeRepository = recipeRepository;
        }
        public async Task<Response<List<PreparedRecipeImage>>> GetByUserIdAsync(int userId,int pageNumber,int pageSize)
        {
            return await preparedRecipeImageRepository.GetByUserIdPagedAsync(userId,GetStartIndex(pageNumber,pageSize),pageSize);
        }

        public async Task<Response<List<PreparedRecipeImage>>> GetByRecipeIdAsync(int recipeId,int pageNumber, int pageSize)
        {
            return await preparedRecipeImageRepository.GetByRecipeIdPagedAsync(recipeId,GetStartIndex(pageNumber,pageSize),pageSize);
        }
        private int GetStartIndex(int pageNumber, int pageSize)
        {
            return (pageNumber - 1) * pageSize;
        }
        public async Task<Response<PreparedRecipeImage>> UploadImage(User creator,PreparedRecipeImageDTO preparedRecipeImageDTO,bool saveChanges=true)
        {
            if (preparedRecipeImageDTO.IsValid())
                return Response<PreparedRecipeImage>.Failure("Image is required", StatusCode.BadRequest);
            var recipeResult = await recipeRepository.GetByIdAsync(preparedRecipeImageDTO.RecipeId);
            if (!recipeResult.IsSuccess)
                return Response<PreparedRecipeImage>.Failure(recipeResult.ErrorMessage, recipeResult.StatusCode);
            PreparedRecipeImage preparedRecipeImage = CreatePreparedRecipeImage(creator,preparedRecipeImageDTO);
            return await preparedRecipeImageRepository.CreateAsync(preparedRecipeImage, saveChanges);
        }

        private  PreparedRecipeImage CreatePreparedRecipeImage(User creator,PreparedRecipeImageDTO preparedRecipeImageDTO)
        {
            return new PreparedRecipeImage
            {
                RecipeId = preparedRecipeImageDTO.RecipeId,
                CreatorId = creator.Id,
                Image = preparedRecipeImageDTO.Image,
                DateUploaded = DateTime.Now
            };
        }


        public async Task<Response<bool>> HasUploadedImageForRecipe(int userId, int recipeId)
        {
            var recipeImagesResult = await GetByUserAndRecipeIdAsync(userId, recipeId);
            if(!recipeImagesResult.IsSuccess)
                return Response<bool>.Failure(recipeImagesResult.ErrorMessage, recipeImagesResult.StatusCode);
            return Response<bool>.Success(recipeImagesResult.Data.Count > 0,StatusCode.OK);
        }

        private async Task<Response<List<PreparedRecipeImage>>> GetByUserAndRecipeIdAsync(int userId, int recipeId)
        {
            return await preparedRecipeImageRepository.GetByUserAndRecipeIdAsync(userId, recipeId);
        }
    }
}
