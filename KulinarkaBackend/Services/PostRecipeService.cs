using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class PostRecipeService
    {
        private readonly IRecipeService recipeService;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IPreparationStepService preparationStepService;
        private readonly IRecipeTagService recipeTagService;
        private readonly IRecipeStatisticsService recipeStatisticsService;
        public PostRecipeService(IRecipeTagService recipeTagService,IRecipeService recipeService,IRecipeIngredientService recipeIngredientService,IPreparationStepService preparationStepService,IRecipeStatisticsService recipeStatisticsService)
        {
            this.recipeService = recipeService;
            this.recipeIngredientService = recipeIngredientService;
            this.preparationStepService = preparationStepService;
            this.recipeTagService = recipeTagService;
            this.recipeStatisticsService = recipeStatisticsService;
        }
        public async Task<Response<Recipe>> AddRecipeAsync(User user,PostRecipeDTO recipeDTO)
        {
            var beginTransaction = await recipeService.BeginTransactionAsync();
            if (!beginTransaction.IsSuccess)
                return Response<Recipe>.Failure(beginTransaction.ErrorMessage, beginTransaction.StatusCode);
            try
            {
                var recipeResult = await recipeService.AddAsync(user, recipeDTO.recipe);
                if (!recipeResult.IsSuccess)
                    throw new Exception(recipeResult.ErrorMessage);
                Recipe newRecipe = recipeResult.Data;

                var recipeIngredientResult = await recipeIngredientService.AddAsync(newRecipe.Id,recipeDTO.RecipeIngredientDTOs,false);
                if(!recipeIngredientResult.IsSuccess)
                    throw new Exception(recipeIngredientResult.ErrorMessage);
                newRecipe.Ingredients = recipeIngredientResult.Data;

                var recipeTagResult = await recipeTagService.AddAsync(newRecipe.Id, recipeDTO.Tags,false);
                if (!recipeTagResult.IsSuccess)
                    throw new Exception(recipeTagResult.ErrorMessage);
                newRecipe.Tags = recipeTagResult.Data;

                var preparationStepResult = await preparationStepService.AddAsync(newRecipe.Id, recipeDTO.PreparationSteps,false);
                if (!preparationStepResult.IsSuccess)
                    throw new Exception(preparationStepResult.ErrorMessage);
                newRecipe.PreparationSteps = preparationStepResult.Data;

                var recipeStatisticsResult = await recipeStatisticsService.AddAsync(newRecipe.Id,false);
                if (!recipeStatisticsResult.IsSuccess)
                    throw new Exception(recipeStatisticsResult.ErrorMessage);
                newRecipe.RecipeStatistics = recipeStatisticsResult.Data;

                var saveChangesResult = await recipeService.SaveChangesAsync();
                if (!saveChangesResult.IsSuccess)
                    throw new Exception(saveChangesResult.ErrorMessage);
                var commitResult = await recipeService.CommitTransactionAsync();
                if (!commitResult.IsSuccess)
                    throw new Exception(commitResult.ErrorMessage);
                return Response<Recipe>.Success(newRecipe, StatusCode.Created);
            }
            catch (Exception e)
            {
                await recipeService.RollbackTransactionAsync();
                return Response<Recipe>.Failure(e.Message, StatusCode.InternalServerError);
            }
        }
    }
}
