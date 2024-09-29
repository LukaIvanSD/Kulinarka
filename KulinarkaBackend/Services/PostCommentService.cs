using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.ServiceInterfaces;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class PostCommentService
    {
        private readonly ICommentService commentService;
        private readonly IRecipeStatisticsService recipeStatisticsService;
        private readonly IPreparedRecipeImageService preparedRecipeImageService;
        private readonly IMapper mapper;
        private readonly IUserTitleService userTitleService;
        public PostCommentService(ICommentService commentService,IRecipeStatisticsService recipeStatisticsService,IPreparedRecipeImageService preparedRecipeImageService,IMapper mapper, IUserTitleService userTitleService)
        {
            this.commentService = commentService;
            this.recipeStatisticsService = recipeStatisticsService;
            this.preparedRecipeImageService = preparedRecipeImageService;
            this.mapper = mapper;
            this.userTitleService = userTitleService;
        }
        public async Task<Response<CommentDTO>> PostCommentAsync(User creator,CreateCommentDTO createCommentDTO)
        {
            var beginTransactionResult = await commentService.BeginTransactionAsync();
            if (!beginTransactionResult.IsSuccess)
                return Response<CommentDTO>.Failure(beginTransactionResult.ErrorMessage,beginTransactionResult.StatusCode);
            try 
            {
                var commentResult = await commentService.CreateAsync(creator, createCommentDTO);
                if (!commentResult.IsSuccess)
                    throw new Exception(commentResult.ErrorMessage);

                var recipeStatisticsResult = await recipeStatisticsService.AddCommentStatisticAsync(createCommentDTO.RecipeId);
                if (!recipeStatisticsResult.IsSuccess)
                    throw new Exception(recipeStatisticsResult.ErrorMessage);
                var commitResult = await commentService.CommitTransactionAsync();
                if (!commitResult.IsSuccess)
                    throw new Exception(commitResult.ErrorMessage);
                var commentDTOResult = await CreateNewCommentDTO(commentResult.Data, creator);
                if (!commentDTOResult.IsSuccess)
                    throw new Exception(commentDTOResult.ErrorMessage);
                return Response<CommentDTO>.Success(commentDTOResult.Data, StatusCode.Created);
            }
            catch (Exception e)
            {
                await commentService.RollbackTransactionAsync();
                return Response<CommentDTO>.Failure(e.Message, StatusCode.InternalServerError);
            }
        }
        public async Task<Response<CommentWithImageResponse>> PostCommentWithPictureAsync(User creator,PostCommentWithImageDTO postCommentWithImageDTO)
        {
            if(postCommentWithImageDTO.IsInvalid())
                return Response<CommentWithImageResponse>.Failure("Invalid image", StatusCode.BadRequest);

            var beginTransactionResult = await commentService.BeginTransactionAsync();
            if (!beginTransactionResult.IsSuccess)
                return Response<CommentWithImageResponse>.Failure(beginTransactionResult.ErrorMessage,beginTransactionResult.StatusCode);
            try
            {
                var preparedImageResult = await preparedRecipeImageService.UploadImage(creator, postCommentWithImageDTO.ToPreparedRecipeImageDTO());
                if (!preparedImageResult.IsSuccess)
                    throw new Exception(preparedImageResult.ErrorMessage);

                var commentResult = await commentService.CreateAsync(creator, postCommentWithImageDTO.ToCreateCommentDTO());
                if (!commentResult.IsSuccess)
                    throw new Exception(commentResult.ErrorMessage);

                var recipeStatisticsResult = await recipeStatisticsService.AddCommentStatisticAsync(postCommentWithImageDTO.RecipeId);
                if (!recipeStatisticsResult.IsSuccess)
                    throw new Exception(recipeStatisticsResult.ErrorMessage);

                var commitResult = await commentService.CommitTransactionAsync();
                if (!commitResult.IsSuccess)
                    throw new Exception(commitResult.ErrorMessage);
                var commentDTOResult  = await CreateNewCommentWithImageDTO(commentResult.Data, creator, preparedImageResult.Data);
                if (!commentDTOResult.IsSuccess)
                    throw new Exception(commentDTOResult.ErrorMessage);
                return Response<CommentWithImageResponse>.Success(commentDTOResult.Data, StatusCode.Created);
            }
            catch (Exception e)
            {
                await commentService.RollbackTransactionAsync();
                return Response<CommentWithImageResponse>.Failure(e.Message, StatusCode.InternalServerError);
            }
        }
        private async Task<Response<CommentDTO>> CreateNewCommentDTO(Comment comment ,User creator) {
            var userTitleResult =await userTitleService.GetUserTitleEagerAsync(creator.Id);
            if (!userTitleResult.IsSuccess)
                return Response<CommentDTO>.Failure(userTitleResult.ErrorMessage, userTitleResult.StatusCode);
            creator.UserTitle = userTitleResult.Data;
            comment.Creator = creator;
            CommentDTO commentDTO = mapper.Map<CommentDTO>(comment);
            return Response<CommentDTO>.Success(commentDTO,StatusCode.OK);
        }
        private async Task<Response<CommentWithImageResponse>> CreateNewCommentWithImageDTO(Comment comment,User creator,PreparedRecipeImageResponse preparedRecipeImageResponse) {
            var commentDTOResult = await CreateNewCommentDTO(comment, creator);
            if (!commentDTOResult.IsSuccess)
                return Response<CommentWithImageResponse>.Failure(commentDTOResult.ErrorMessage, commentDTOResult.StatusCode);
            Debug.WriteLine(commentDTOResult.Data.CreatorName);
            Debug.WriteLine(preparedRecipeImageResponse.CreatorUsername);
            CommentWithImageResponse commentWithImageResponse = new CommentWithImageResponse(commentDTOResult.Data, preparedRecipeImageResponse);
            return Response<CommentWithImageResponse>.Success(commentWithImageResponse, StatusCode.OK);
        }
    }
}
