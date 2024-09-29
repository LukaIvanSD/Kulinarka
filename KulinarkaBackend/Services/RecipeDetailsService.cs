using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.ServiceInterfaces;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class RecipeDetailsService : IRecipeDetailsService
    {
        private readonly IMapper mapper;
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IPreparationStepService preparationStepService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IRecipeTagService recipeTagService;
        private readonly ICommentService commentService;
        private readonly IPreparedRecipeImageService preparedRecipeImageService;
        public RecipeDetailsService(IMapper mapper, IRecipeService recipeService,IUserService userService,IPreparationStepService preparationStepService,IRecipeIngredientService recipeIngredientService,IRecipeTagService recipeTagService,ICommentService commentService,IPreparedRecipeImageService preparedRecipeImageService)
        {
            this.mapper = mapper;
            this.recipeService = recipeService;
            this.userService = userService;
            this.preparationStepService = preparationStepService;
            this.recipeIngredientService = recipeIngredientService;
            this.recipeTagService = recipeTagService;
            this.preparedRecipeImageService = preparedRecipeImageService;
            this.commentService = commentService;
        }
        public async Task<Response<RecipeDetailsDTO>> GetRecipeDetails(int recipeId)
        {
            var recipeResult = await recipeService.GetByIdWithDetailsAsync(recipeId);
            if (!recipeResult.IsSuccess)
                return Response<RecipeDetailsDTO>.Failure(recipeResult.ErrorMessage,recipeResult.StatusCode);
            Recipe recipe = recipeResult.Data;
            var userResult = await userService.GetUserAndTitleEagerAsync(recipe.UserId);
            if (!userResult.IsSuccess)
                return Response<RecipeDetailsDTO>.Failure(userResult.ErrorMessage, userResult.StatusCode);
            recipe.User = userResult.Data;
            int PageNumber = 1;
            int PageSize = 5;
            //Gets first 2 pages of comments
            var commentsResult = await commentService.GetByRecipePagedWithCreatorEagerAsync(recipe,PageNumber,PageSize);
            if (!commentsResult.IsSuccess)
                return Response<RecipeDetailsDTO>.Failure(commentsResult.ErrorMessage, commentsResult.StatusCode);
            recipe.Comments = commentsResult.Data;
            var preparedRecipeImagesResult = await preparedRecipeImageService.GetByRecipeIdWithCreatorEagerAsync(recipe.Id,PageNumber,PageSize);
            if (!preparedRecipeImagesResult.IsSuccess)
                return Response<RecipeDetailsDTO>.Failure(preparedRecipeImagesResult.ErrorMessage, preparedRecipeImagesResult.StatusCode);
            recipe.PreparedRecipeImages = preparedRecipeImagesResult.Data;
            RecipeDetailsDTO recipeDetailsDTO = mapper.Map<RecipeDetailsDTO>(recipe);
            return Response<RecipeDetailsDTO>.Success(recipeDetailsDTO, StatusCode.OK);

        }

        public async Task<Response<RecipeDetailsDTO>> UpdateRecipeDetailsAsync(User user, RecipeDetailsDTO recipeDetailsDTO)
        {
            var ownerResult = await recipeService.IsUserOwnerOfRecipe(user, recipeDetailsDTO.Recipe.Id);
            if (!ownerResult.IsSuccess)
                return Response<RecipeDetailsDTO>.Failure(ownerResult.ErrorMessage, ownerResult.StatusCode);
            var recipeResult = await recipeService.GetByIdWithDetailsAsync(recipeDetailsDTO.Recipe.Id);
            if (!recipeResult.IsSuccess)
                return Response<RecipeDetailsDTO>.Failure(recipeResult.ErrorMessage, recipeResult.StatusCode);
            Recipe oldRecipe = recipeResult.Data;


            var beginTransactionResult = await recipeService.BeginTransactionAsync();
            if (!beginTransactionResult.IsSuccess)

                return Response<RecipeDetailsDTO>.Failure(beginTransactionResult.ErrorMessage, beginTransactionResult.StatusCode);
            try
            {
                mapper.Map(recipeDetailsDTO.Recipe, oldRecipe);

                var updateRecipeResult = await recipeService.UpdateWithDetailsAsync(oldRecipe, false);
                if (!updateRecipeResult.IsSuccess)
                    throw new Exception(updateRecipeResult.ErrorMessage);

                var updatePreparationStepsResult = await preparationStepService.UpdateAsync(oldRecipe.PreparationSteps.ToList(), recipeDetailsDTO.Steps,false);
                if (!updatePreparationStepsResult.IsSuccess)
                    throw new Exception(updatePreparationStepsResult.ErrorMessage);

                var updateIngredientsResult = await recipeIngredientService.UpdateAsync(oldRecipe.Ingredients.ToList(), recipeDetailsDTO.Ingredients,false);
                if (!updateIngredientsResult.IsSuccess)
                    throw new Exception(updateIngredientsResult.ErrorMessage);

                var updateTagsResult = await recipeTagService.UpdateAsync(oldRecipe.Tags.ToList(), recipeDetailsDTO.Tags,false);
                if (!updateTagsResult.IsSuccess)
                    throw new Exception(updateTagsResult.ErrorMessage);

                var saveChangesResult = await recipeService.SaveChangesAsync();
                if (!saveChangesResult.IsSuccess)
                    throw new Exception(saveChangesResult.ErrorMessage);

                var commitResult = await recipeService.CommitTransactionAsync();
                if (!commitResult.IsSuccess)
                    throw new Exception(commitResult.ErrorMessage);

                return Response<RecipeDetailsDTO>.Success(recipeDetailsDTO, StatusCode.OK);
            }
            catch (Exception ex)
            { 
                await recipeService.RollbackTransactionAsync();
                return Response<RecipeDetailsDTO>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
