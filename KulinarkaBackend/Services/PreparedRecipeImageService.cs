using AutoMapper;
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
        private readonly IMapper mapper;
        public PreparedRecipeImageService(IPreparedRecipeImageRepository preparedRecipeImageRepository,IRecipeRepository recipeRepository,IMapper mapper) {
            this.preparedRecipeImageRepository = preparedRecipeImageRepository;
            this.recipeRepository = recipeRepository;
            this.mapper = mapper;
        }
        public async Task<Response<List<UserPreparedRecipeImageResponse>>> GetByUserIdAsync(int userId,int pageNumber,int pageSize)
        {
           var preparedRecipeImagesResult = await preparedRecipeImageRepository.GetByUserIdPagedAsync(userId,GetStartIndex(pageNumber,pageSize),pageSize);
            if (!preparedRecipeImagesResult.IsSuccess)
                return Response<List<UserPreparedRecipeImageResponse>>.Failure(preparedRecipeImagesResult.ErrorMessage, preparedRecipeImagesResult.StatusCode);
            List<UserPreparedRecipeImageResponse> preparedRecipeImageResponses = mapper.Map<List<UserPreparedRecipeImageResponse>>(preparedRecipeImagesResult.Data);
            return Response<List<UserPreparedRecipeImageResponse>>.Success(preparedRecipeImageResponses,StatusCode.OK);
        }

        public async Task<Response<List<PreparedRecipeImageResponse>>> GetByRecipeIdAsync(int recipeId,int pageNumber, int pageSize)
        {
            var preparedRecipeImageResult =await GetByRecipeIdWithCreatorEagerAsync(recipeId,GetStartIndex(pageNumber,pageSize),pageSize);
            if (!preparedRecipeImageResult.IsSuccess)
                return Response<List<PreparedRecipeImageResponse>>.Failure(preparedRecipeImageResult.ErrorMessage, preparedRecipeImageResult.StatusCode);
            List<PreparedRecipeImageResponse> preparedRecipeImageResponses = mapper.Map<List<PreparedRecipeImageResponse>>(preparedRecipeImageResult.Data);
            return Response<List<PreparedRecipeImageResponse>>.Success(preparedRecipeImageResponses, StatusCode.OK);
        }
        private int GetStartIndex(int pageNumber, int pageSize)
        {
            return (pageNumber - 1) * pageSize;
        }
        public async Task<Response<PreparedRecipeImageResponse>> UploadImage(User creator,PreparedRecipeImageDTO preparedRecipeImageDTO,bool saveChanges=true)
        {
            if (preparedRecipeImageDTO.IsValid())
                return Response<PreparedRecipeImageResponse>.Failure("Image is required", StatusCode.BadRequest);
            var recipeResult = await recipeRepository.GetByIdAsync(preparedRecipeImageDTO.RecipeId);
            if (!recipeResult.IsSuccess)
                return Response<PreparedRecipeImageResponse>.Failure(recipeResult.ErrorMessage, recipeResult.StatusCode);
            PreparedRecipeImage preparedRecipeImage = CreatePreparedRecipeImage(creator,preparedRecipeImageDTO);
            var prepareRecipeImageResult = await preparedRecipeImageRepository.CreateAsync(preparedRecipeImage, saveChanges);
            if(!prepareRecipeImageResult.IsSuccess)
                return Response<PreparedRecipeImageResponse>.Failure(prepareRecipeImageResult.ErrorMessage, prepareRecipeImageResult.StatusCode);
            preparedRecipeImage = prepareRecipeImageResult.Data;
            preparedRecipeImage.Creator = creator;
            PreparedRecipeImageResponse response = mapper.Map<PreparedRecipeImageResponse>(prepareRecipeImageResult.Data);
            return Response<PreparedRecipeImageResponse>.Success(response, StatusCode.Created);
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

        public async Task<Response<List<PreparedRecipeImage>>> GetByRecipeIdWithCreatorEagerAsync(int id, int pageNumber, int pageSize)
        {
            return await preparedRecipeImageRepository.GetByRecipeIdPagedWithCreatorEagerAsync(id, GetStartIndex(pageNumber, pageSize), pageSize);
        }
    }
}
