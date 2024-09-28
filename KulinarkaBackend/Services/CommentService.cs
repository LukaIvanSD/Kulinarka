using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepository;
        private readonly IRecipeRepository recipeRepository;
        private readonly IMapper mapper;
        private readonly IPreparedRecipeImageService preparedRecipeImageService;
        public CommentService(ICommentRepository commentRepository,IRecipeRepository recipeRepository,IMapper mapper,IPreparedRecipeImageService preparedRecipeImageService)
        {
            this.mapper = mapper;
            this.recipeRepository = recipeRepository;
            this.commentRepository = commentRepository;
            this.preparedRecipeImageService = preparedRecipeImageService;
        }
        public async Task<Response<List<Comment>>> GetByRecipeIdPagedAsync(int recipeId,int pageNumber,int pageSize)
        {
            var recipeResult = await recipeRepository.GetByIdAsync(recipeId);
            if (recipeResult == null)
                return Response<List<Comment>>.Failure("Recipe not found", StatusCode.NotFound);
            return await commentRepository.GetByRecipeIdPagedAsync(recipeId,GetStartIndex(pageNumber,pageSize), pageSize);
        }
        public async Task<Response<List<Comment>>> GetByRecipePagedAsync(Recipe recipe, int pageNumber, int pageSize)
        {
            return await commentRepository.GetByRecipeIdPagedAsync(recipe.Id, GetStartIndex(pageNumber, pageSize), pageSize);
        }
        public async Task<Response<Comment>> CreateAsync(User creator,CreateCommentDTO createCommentDTO)
        {
            var recipeResult = await recipeRepository.GetByIdAsync(createCommentDTO.RecipeId);
            if (recipeResult == null)
                return Response<Comment>.Failure("Recipe not found", StatusCode.NotFound);
            var hasUploaded =await preparedRecipeImageService.HasUploadedImageForRecipe(creator.Id, createCommentDTO.RecipeId);
            if(!hasUploaded.IsSuccess)
                return Response<Comment>.Failure(hasUploaded.ErrorMessage, hasUploaded.StatusCode);
            if (!hasUploaded.Data)
                return Response<Comment>.Failure("You didnt upload recipe image", StatusCode.NotFound);
            Comment comment = mapper.Map<Comment>(createCommentDTO,opt=>
            opt.Items["CreatorId"]=creator.Id);
            return await commentRepository.CreateAsync(comment);
        }

        private int GetStartIndex(int pageNumber,int pageSize)
        {
            return (pageNumber - 1) * pageSize;
        }
    }
}
