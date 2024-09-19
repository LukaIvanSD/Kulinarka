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
        public async Task<Response<List<RecipeIngredient>>> AddAsync(int recipeId, List<RecipeIngredientDTO> recipeIngredientDTOs)
        {
            List<RecipeIngredient> recipeIngredients = new List<RecipeIngredient>();
            foreach (RecipeIngredientDTO recipeIngredientDTO in recipeIngredientDTOs)
            {
                var ingredientResult = await ingredientService.GetIngredientByName(recipeIngredientDTO.Name);
                if (!ingredientResult.IsSuccess)
                    return Response<List<RecipeIngredient>>.Failure(ingredientResult.ErrorMessage, ingredientResult.StatusCode);
                RecipeIngredient recipeIngredient = GenerateRecipeIngredient(recipeId, ingredientResult.Data.Id,recipeIngredientDTO);
                recipeIngredients.Add(recipeIngredient);
                var recipeIngredientResult = await recipeIngredientRepository.CreateAsync(recipeIngredient,false);
                if (!recipeIngredientResult.IsSuccess)
                    return Response<List<RecipeIngredient>>.Failure(recipeIngredientResult.ErrorMessage, recipeIngredientResult.StatusCode);
            }
            return Response<List<RecipeIngredient>>.Success(recipeIngredients, StatusCode.Created);
        }

        private RecipeIngredient GenerateRecipeIngredient(int recipeId,int ingredientId, RecipeIngredientDTO recipeIngredientDTO)
        {
            return new RecipeIngredient(recipeId, ingredientId, recipeIngredientDTO.Amount, recipeIngredientDTO.MeasurementUnit);
        }
    }
}
