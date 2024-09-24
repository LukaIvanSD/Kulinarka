﻿using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Services;
using Microsoft.AspNetCore.Mvc;
using Kulinarka.Models.Responses;
using AutoMapper;
using Kulinarka.DTO;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly ILoginService loginService;
        private readonly ISessionService sessionService;
        private readonly IMapper mapper;
        private readonly PostRecipeService postRecipeService;
        public RecipesController(IRecipeService recipeService, ILoginService loginService, ISessionService sessionService, IMapper mapper, PostRecipeService postRecipeService)
        {
            this.postRecipeService = postRecipeService;
            this.mapper = mapper;
            this.recipeService = recipeService;
            this.loginService = loginService;
            this.sessionService = sessionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var result = await recipeService.GetAllAsync();
            return HandleResponse(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var result = await recipeService.GetByIdWithDetailsAsync(id);
            return HandleResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromForm] RecipeUploadDTO recipeUploadDTO)
        {
            var recipe = JsonConvert.DeserializeObject<Recipe>(recipeUploadDTO.Recipe);
            var ingredients = JsonConvert.DeserializeObject<List<RecipeIngredientDTO>>(recipeUploadDTO.Ingredients);
            var steps = JsonConvert.DeserializeObject<List<PreparationStepDTO>>(recipeUploadDTO.PreparationSteps);
            var tags = JsonConvert.DeserializeObject<List<Tag>>(recipeUploadDTO.Tags);
            if (tags == null || ingredients==null || steps==null || recipe==null)
                return BadRequest();
            if (recipeUploadDTO.Picture == null)
                return BadRequest();
            using (var pictureStream = new MemoryStream())
            {
                await recipeUploadDTO.Picture.CopyToAsync(pictureStream);
                recipe.Picture = pictureStream.ToArray();
            }

            if (recipeUploadDTO.Video != null)
            {
                using (var videoStream = new MemoryStream())
                {
                    await recipeUploadDTO.Video.CopyToAsync(videoStream);
                    recipe.VideoData = videoStream.ToArray();
                    recipe.ContentType = recipeUploadDTO.ContentType;
                }
            }
            PostRecipeDTO recipeDTO = new PostRecipeDTO(recipe, ingredients, steps, tags);
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await postRecipeService.AddRecipeAsync(loginResult.Data, recipeDTO);
            return HandleResponse(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeDTO recipeDTO)
        {
            if (id != recipeDTO.Id)
                return BadRequest();
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            Recipe recipe = mapper.Map<Recipe>(recipeDTO);
            var result = await recipeService.UpdateAsync(loginResult.Data, recipe);
            return HandleResponse(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await recipeService.DeleteAsync(loginResult.Data, id);
            return HandleResponse(result);
        }
        [HttpGet("sorted")]
        public async Task<IActionResult> GetAllSorted() 
        {
            var result = await recipeService.GetSortedAsync();
            return HandleResponse(result);
        }
        [HttpGet("user")]
        public async Task<IActionResult> GetUserRecipes()
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await recipeService.GetUserRecipesAsync(loginResult.Data);
            return HandleResponse(result);
        }
        [HttpGet("IsUserOwnerOfRecipe/{recipeid}")]
        public async Task<IActionResult> IsUserOwnerOfRecipe(int recipeid)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await recipeService.IsUserOwnerOfRecipe(loginResult.Data,recipeid);
            return HandleResponse(result);
        }


    }
}
