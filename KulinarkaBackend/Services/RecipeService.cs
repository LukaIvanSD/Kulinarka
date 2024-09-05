using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Kulinarka.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository recipeRepository;
        public RecipeService(IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }
        public async Task<Response<List<Recipe>>> GetRecipesAsync()
        {
            return await recipeRepository.GetAllAsync();
        }
        public async Task<Response<Recipe>> GetRecipeAsync(int id)
        {
            return await recipeRepository.GetByIdAsync(id);
        }
        public async Task<Response<Recipe>> AddRecipeAsync(User user ,Recipe recipe)
        {
            recipe.UserId = user.Id;
            recipe.CreationDate = DateTime.Now;
            return await recipeRepository.CreateAsync(recipe);

        }
        public async Task<Response<Recipe>> UpdateRecipeAsync(User user,Recipe recipe)
        {
            if (user.Id != recipe.UserId)
                return Response<Recipe>.Failure("User is not the owner of the recipe",StatusCode.BadRequest);
            return await recipeRepository.UpdateAsync(recipe.Id,recipe);
        }
        public async Task<Response<Recipe>> DeleteRecipeAsync(User user ,int recipeId)
        {
            var result = await recipeRepository.GetByIdAsync(recipeId);
            if (!result.IsSuccess)
                return result;
            Recipe recipe = result.Data;
            if (user.Id != recipe.UserId)
                return Response<Recipe>.Failure("User is not the owner of the recipe", StatusCode.BadRequest);
            return await recipeRepository.DeleteAsync(recipe.Id);
        }
    }
}
