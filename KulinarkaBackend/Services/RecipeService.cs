using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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
        public async Task<Response<List<Recipe>>> GetAllAsync()
        {
            return await recipeRepository.GetAllAsync();
        }
        public async Task<Response<Recipe>> GetByIdAsync(int id)
        {
            return await recipeRepository.GetByIdAsync(id);
        }
        public async Task<Response<Recipe>> AddAsync(User user ,Recipe recipe,bool saveChanges=true)
        {
            recipe.UserId = user.Id;
            recipe.CreationDate = DateTime.Now;
            return await recipeRepository.CreateAsync(recipe,saveChanges);

        }
        public async Task<Response<Recipe>> UpdateAsync(User user,Recipe newRecipe)
        {
            var recipeResult = await recipeRepository.GetByIdAsync(newRecipe.Id);
            if (!recipeResult.IsSuccess)
                return recipeResult;
            if (user.Id != recipeResult.Data.UserId)
                return Response<Recipe>.Failure("User is not the owner of the recipe", StatusCode.BadRequest);
            return await recipeRepository.UpdateAsync(newRecipe.Id, newRecipe);
        }
        public async Task<Response<Recipe>> DeleteAsync(User user ,int recipeId)
        {
            var result = await recipeRepository.GetByIdAsync(recipeId);
            if (!result.IsSuccess)
                return result;
            Recipe recipe = result.Data;
            if (user.Id != recipe.UserId)
                return Response<Recipe>.Failure("User is not the owner of the recipe", StatusCode.BadRequest);
            return await recipeRepository.DeleteAsync(recipe.Id);
        }

        public async Task<Response<List<SortedRecipesDTO>>> GetSortedAsync()
        {
            var result = await recipeRepository.GetRecipesAndPromotionsEagerAsync();
            if (!result.IsSuccess)
                return Response<List<SortedRecipesDTO>>.Failure(result.ErrorMessage, result.StatusCode);
            List<Recipe> sortedRecipes = SortRecipes(result.Data).Result;
            List<SortedRecipesDTO> sortedRecipesDTO = CreateSortedRecipesDTO(sortedRecipes);
            return Response<List<SortedRecipesDTO>>.Success(sortedRecipesDTO,StatusCode.OK);
        }

        private List<SortedRecipesDTO> CreateSortedRecipesDTO(List<Recipe> sortedRecipes)
        {
            List<SortedRecipesDTO> sortedRecipesDTO=new List<SortedRecipesDTO>();
            foreach (Recipe recipe in sortedRecipes)
            {
                SortedRecipesDTO dto = new SortedRecipesDTO(recipe, recipe.IsPromoted());
                sortedRecipesDTO.Add(dto);
            }
            return sortedRecipesDTO;
        }

        private  Task<List<Recipe>> SortRecipes(List<Recipe> recipes)
        {
            return Task.FromResult(recipes.OrderByDescending(r => r.DatePromoted()).ToList());
        }

        public async Task<Response<List<SortedRecipesDTO>>> GetUserRecipesAsync(User user)
        {
            var result = await GetUserRecipesWithPromotionsEagerAsync(user);
            if (!result.IsSuccess)
                return Response<List<SortedRecipesDTO>>.Failure(result.ErrorMessage, result.StatusCode);
            List<Recipe> sortedRecipes = SortRecipes(result.Data).Result;
            List<SortedRecipesDTO> sortedRecipesDTO = CreateSortedRecipesDTO(sortedRecipes);
            return Response<List<SortedRecipesDTO>>.Success(sortedRecipesDTO, StatusCode.OK);
        }

        public async Task<Response<List<Recipe>>> GetUserRecipesWithPromotionsEagerAsync(User user)
        {
            var result = await recipeRepository.GetUserRecipesWithPromotionsEagerAsync(user.Id);
            return result;
        }

        public async Task<Response<Recipe>> BeginTransactionAsync()
        {
            return await recipeRepository.BeginTransactionAsync();
        }

        public async Task<Response<Recipe>> CommitTransactionAsync()
        {
            return await recipeRepository.CommitTransactionAsync();
        }

        public async Task<Response<Recipe>> RollbackTransactionAsync()
        {
            return await recipeRepository.RollbackTransactionAsync();
        }

        public async Task<Response<Recipe>> SaveChangesAsync()
        {
            return await recipeRepository.SaveChangesAsync();
        }
        public async Task<Response<RecipeDTO>> GetByIdWithDetailsAsync(int id)
        {
            var recipeResult = await recipeRepository.GetRecipeDetailsEagerAsync(id);
            if (!recipeResult.IsSuccess)
                return Response<RecipeDTO>.Failure(recipeResult.ErrorMessage, recipeResult.StatusCode);
            if (recipeResult.Data == null)
                return Response<RecipeDTO>.Failure("Recipe not found", StatusCode.NotFound);
            return Response<RecipeDTO>.Success(new RecipeDTO(recipeResult.Data), StatusCode.OK);
        }
    }
}
