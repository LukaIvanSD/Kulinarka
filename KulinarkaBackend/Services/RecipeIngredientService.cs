using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly IRecipeIngredientRepository recipeIngredientRepository;
        private readonly IIngredientService ingredientService;
        public RecipeIngredientService(IRecipeIngredientRepository recipeIngredientRepository,IIngredientService ingredientService)
        {
            this.recipeIngredientRepository = recipeIngredientRepository;
            this.ingredientService = ingredientService;
        }
        public async Task<Response<List<RecipeIngredient>>> AddAsync(int recipeId, List<RecipeIngredientDTO> recipeIngredientDTOs,bool saveChanges=true)
        {
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
            foreach (RecipeIngredientDTO recipeIngredientDTO in recipeIngredientDTOs)
            {
                var ingredientResult = await ingredientService.GetIngredientByName(recipeIngredientDTO.Name);
                if (!ingredientResult.IsSuccess)
                    return Response<List<RecipeIngredient>>.Failure(ingredientResult.ErrorMessage, ingredientResult.StatusCode);
                RecipeIngredient recipeIngredient = GenerateRecipeIngredient(recipeId, ingredientResult.Data.Id,recipeIngredientDTO);
                recipeIngredients.Add(recipeIngredient);
                var recipeIngredientResult = await recipeIngredientRepository.CreateAsync(recipeIngredient, saveChanges);
                if (!recipeIngredientResult.IsSuccess)
                    return Response<List<RecipeIngredient>>.Failure(recipeIngredientResult.ErrorMessage, recipeIngredientResult.StatusCode);
            }
            return Response<List<RecipeIngredient>>.Success(recipeIngredients, StatusCode.Created);
        }
        private async Task<Response<RecipeIngredient>> AddAsync(int recipeId, RecipeIngredientDTO recipeIngredientDTO, bool saveChanges = true)
        {
                var ingredientResult = await ingredientService.GetIngredientByName(recipeIngredientDTO.Name);
                if (!ingredientResult.IsSuccess)
                    return Response<RecipeIngredient>.Failure(ingredientResult.ErrorMessage, ingredientResult.StatusCode);
                RecipeIngredient recipeIngredient = GenerateRecipeIngredient(recipeId, ingredientResult.Data.Id, recipeIngredientDTO);
                var recipeIngredientResult = await recipeIngredientRepository.CreateAsync(recipeIngredient, saveChanges);
                return recipeIngredientResult;
        }

        public async Task<Response<List<RecipeIngredient>>> GetRecipeIngredientsAsync(int recipeId)
        {
            return await recipeIngredientRepository.GetByRecipeIdEagerAsync(recipeId);
        }

        public async Task<Response<List<RecipeIngredient>>> UpdateAsync(List<RecipeIngredient> oldRecipeIngredients, List<RecipeIngredientDTO> newRecipeIngredientsDTO, bool saveChanges = true)
        {
            int recipeId= oldRecipeIngredients[0].RecipeId;
            List<RecipeIngredient> newRecipeIngredients= new List<RecipeIngredient>();
            foreach (RecipeIngredientDTO recipeIngredientDTO in newRecipeIngredientsDTO)
            {
                RecipeIngredient existingRecipeIngredient = oldRecipeIngredients.Find(ri => ri.Ingredient.Name == recipeIngredientDTO.Name);
                if (existingRecipeIngredient != null)
                {
                    var updateResult = await UpdateExistingAsync(existingRecipeIngredient, recipeIngredientDTO,saveChanges);
                    if(!updateResult.IsSuccess)
                        return Response<List<RecipeIngredient>>.Failure(updateResult.ErrorMessage, updateResult.StatusCode);
                    continue;
                }
                    var addRecipeIngredientResult = await AddAsync(recipeId,recipeIngredientDTO, saveChanges);
                if (!addRecipeIngredientResult.IsSuccess)
                    return Response<List<RecipeIngredient>>.Failure(addRecipeIngredientResult.ErrorMessage, addRecipeIngredientResult.StatusCode);
            }
            foreach (RecipeIngredient recipeIngredient in oldRecipeIngredients)
                if (!newRecipeIngredientsDTO.Any(riDTO => riDTO.Name == recipeIngredient.Ingredient.Name))
                {
                    var deleteResult = await recipeIngredientRepository.DeleteAsync(recipeIngredient.Id, saveChanges);
                    if (!deleteResult.IsSuccess)
                        return Response<List<RecipeIngredient>>.Failure(deleteResult.ErrorMessage, deleteResult.StatusCode);
                }

            return Response<List<RecipeIngredient>>.Success(newRecipeIngredients, StatusCode.OK);
        }

        private async Task<Response<RecipeIngredient>> UpdateExistingAsync(RecipeIngredient existingRecipeIngredient, RecipeIngredientDTO recipeIngredientDTO, bool saveChanges)
        {
            if(existingRecipeIngredient.Amount==recipeIngredientDTO.Amount && existingRecipeIngredient.MeasurementUnit ==recipeIngredientDTO.MeasurementUnit)
                return Response<RecipeIngredient>.Success(existingRecipeIngredient, StatusCode.OK);
            existingRecipeIngredient.Amount = recipeIngredientDTO.Amount;
            existingRecipeIngredient.MeasurementUnit = recipeIngredientDTO.MeasurementUnit;
            return await recipeIngredientRepository.UpdateAsync(existingRecipeIngredient.Id, existingRecipeIngredient, saveChanges);
        }

        private RecipeIngredient GenerateRecipeIngredient(int recipeId,int ingredientId, RecipeIngredientDTO recipeIngredientDTO)
        {
            return new RecipeIngredient(recipeId, ingredientId, recipeIngredientDTO.Amount, recipeIngredientDTO.MeasurementUnit);
        }
    }
}
