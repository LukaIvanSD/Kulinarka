using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Kulinarka.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IMapper mapper;
        private readonly IRecipeIngredientService recipeIngredientService;
        private readonly IRecipeTagService recipeTagService;
        public RecipeService(IRecipeRepository recipeRepository,IMapper mapper, IRecipeIngredientService recipeIngredientService, IRecipeTagService recipeTagService)
        {
            this.recipeIngredientService = recipeIngredientService;
            this.recipeRepository = recipeRepository;
            this.mapper = mapper;
            this.recipeTagService = recipeTagService;
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
            var result = await recipeRepository.GetRecipesAndPromotionsAndOwnerEagerAsync();
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
                SortedRecipesDTO dto = new SortedRecipesDTO(recipe, recipe.IsPromoted(),recipe.User.Username);
                sortedRecipesDTO.Add(dto);
            }
            return sortedRecipesDTO;
        }

        private  Task<List<Recipe>> SortRecipes(List<Recipe> recipes)
        {
            return Task.FromResult(recipes.OrderByDescending(r => r.DatePromoted()).ToList());
        }

        public async Task<Response<List<UserRecipeDTO>>> GetUserRecipesAsync(User user)
        {
            var result = await GetUserRecipesWithPromotionsEagerAsync(user);
            if (!result.IsSuccess)
                return Response<List<UserRecipeDTO>>.Failure(result.ErrorMessage, result.StatusCode);
            List<Recipe> userRecipes = result.Data;
            List<UserRecipeDTO> userRecipeDTO = new List<UserRecipeDTO>();
            foreach (Recipe userRecipe in userRecipes)
            {
                var ingredientsResult = await recipeIngredientService.GetRecipeIngredientsAsync(userRecipe.Id);
                if (!ingredientsResult.IsSuccess)
                    return Response<List<UserRecipeDTO>>.Failure(ingredientsResult.ErrorMessage, ingredientsResult.StatusCode);
                if (ingredientsResult.Data == null)
                    return Response<List<UserRecipeDTO>>.Failure("No ingredients found", StatusCode.BadRequest);
                userRecipe.Ingredients = ingredientsResult.Data;
                var tagsResult = await recipeTagService.GetByRecipeIdAsync(userRecipe.Id);
                if (!tagsResult.IsSuccess)
                    return Response<List<UserRecipeDTO>>.Failure(tagsResult.ErrorMessage, tagsResult.StatusCode);
                if (tagsResult.Data == null)
                    return Response<List<UserRecipeDTO>>.Failure("No tags found", StatusCode.BadRequest);
                userRecipe.Tags = tagsResult.Data;
                userRecipeDTO.Add(mapper.Map<UserRecipeDTO>(userRecipe, opt => {
                    opt.Items["IsPromoted"] = userRecipe.IsPromoted();
                }));
            }
            
            return Response<List<UserRecipeDTO>>.Success(userRecipeDTO, StatusCode.OK);
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
        public async Task<Response<Recipe>> GetByIdWithDetailsAsync(int id)
        {
            var recipeResult = await recipeRepository.GetRecipeDetailsEagerAsync(id);
            if (!recipeResult.IsSuccess)
                return Response<Recipe>.Failure(recipeResult.ErrorMessage, recipeResult.StatusCode);
            if (recipeResult.Data == null)
                return Response<Recipe>.Failure("Recipe not found", StatusCode.NotFound);
            return Response<Recipe>.Success(recipeResult.Data, StatusCode.OK);
        }

        public async Task<Response<Recipe>> UpdateWithDetailsAsync(Recipe newRecipe,bool saveChanges=true)
        {
            return await recipeRepository.UpdateAsync(newRecipe.Id, newRecipe,saveChanges);
        }

        public async Task<Response<int>> CountUserRecipes(int userId)
        {
            return await recipeRepository.CountUserRecipes(userId);
        }

        public async Task<Response<bool>> IsUserOwnerOfRecipe(User user, int recipeid)
        {
            var result = await recipeRepository.GetByIdAsync(recipeid);
            if (!result.IsSuccess)
                return Response<bool>.Failure(result.ErrorMessage, result.StatusCode);
            if (result.Data.UserId != user.Id)
                return Response<bool>.Success(false, StatusCode.OK);
            return Response<bool>.Success(true, StatusCode.OK);
        }
    }
}
