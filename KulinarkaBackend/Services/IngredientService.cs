using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository ingredientRepository;
        public IngredientService(IIngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }
        public async Task<Response<Ingredient>> GetIngredientByName(string ingredientName)
        {
            var ingredientResult = await GetOrCreateIngredient(ingredientName);
            if (!ingredientResult.IsSuccess)
                return Response<Ingredient>.Failure(ingredientResult.ErrorMessage, ingredientResult.StatusCode);
            return Response<Ingredient>.Success(ingredientResult.Data, StatusCode.OK);
        }

        private async Task<Response<Ingredient>> GetOrCreateIngredient(string ingredientName)
        {
            var ingredientResult = await ingredientRepository.GetByNameAsync(ingredientName);
            if (!ingredientResult.IsSuccess)
                return ingredientResult;
            if (ingredientResult.Data != null)
                return ingredientResult;
            var newIngredientResult = await ingredientRepository.CreateAsync(new Ingredient(ingredientName));
            return newIngredientResult;
        }
    }
}
