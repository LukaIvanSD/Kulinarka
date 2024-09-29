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
        public async Task<Response<List<CommentDTO>>> GetByRecipeIdPagedAsync(int recipeId,int pageNumber,int pageSize)
        {
            var recipeResult = await recipeRepository.GetByIdAsync(recipeId);
            if (recipeResult == null)
                return Response<List<CommentDTO>>.Failure("Recipe not found", StatusCode.NotFound);
             var commentsResult = await GetByRecipePagedWithCreatorEagerAsync(recipeResult.Data, pageNumber, pageSize);
            if (!commentsResult.IsSuccess)
                return Response<List<CommentDTO>>.Failure(commentsResult.ErrorMessage, commentsResult.StatusCode);
            List<CommentDTO> commentDTOs = mapper.Map<List<CommentDTO>>(commentsResult.Data);
            return Response<List<CommentDTO>>.Success(commentDTOs, StatusCode.OK);
        }
        public async Task<Response<List<Comment>>> GetByRecipePagedAsync(Recipe recipe, int pageNumber, int pageSize)
        {
            return await commentRepository.GetByRecipeIdPagedAsync(recipe.Id, GetStartIndex(pageNumber, pageSize), pageSize);
        }
        public async Task<Response<Comment>> CreateAsync(User creator,CreateCommentDTO createCommentDTO, bool saveChanges = true)
        {
            var recipeResult = await recipeRepository.GetByIdAsync(createCommentDTO.RecipeId);
            if (recipeResult == null)
                return Response<Comment>.Failure("Recipe not found", StatusCode.NotFound);
            if(recipeResult.Data.IsUserOwnerOfRecipe(creator.Id))
                return Response<Comment>.Failure("You cant comment on your own recipe", StatusCode.BadRequest);
            var hasUploaded =await preparedRecipeImageService.HasUploadedImageForRecipe(creator.Id, createCommentDTO.RecipeId);
            if(!hasUploaded.IsSuccess)
                return Response<Comment>.Failure(hasUploaded.ErrorMessage, hasUploaded.StatusCode);
            if (!hasUploaded.Data)
                return Response<Comment>.Failure("You didnt upload recipe image", StatusCode.NotFound);
            Comment comment = mapper.Map<Comment>(createCommentDTO,opt=>
            opt.Items["CreatorId"]=creator.Id);
            return await commentRepository.CreateAsync(comment,saveChanges);
        }

        private int GetStartIndex(int pageNumber,int pageSize)
        {
            return (pageNumber - 1) * pageSize;
        }

        public async Task<Response<Comment>> RollbackTransactionAsync()
        {
            return await commentRepository.RollbackTransactionAsync();
        }

        public async Task<Response<Comment>> BeginTransactionAsync()
        {
            return await commentRepository.BeginTransactionAsync();
        }

        public async Task<Response<Comment>> SaveChangesAsync()
        {
            return await commentRepository.SaveChangesAsync();
        }

        public async Task<Response<Comment>> CommitTransactionAsync()
        {
            return await commentRepository.CommitTransactionAsync();
        }

        public async Task<Response<List<Comment>>> GetByRecipePagedWithCreatorEagerAsync(Recipe recipe, int pageNumber, int pageSize)
        {
            return await commentRepository.GetByRecipeIdPagedWithCreatorEagerAsync(recipe.Id, GetStartIndex(pageNumber, pageSize), pageSize);
        }
    }
}
