using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Services;
using Microsoft.AspNetCore.Mvc;
using Kulinarka.Models.Responses;
using AutoMapper;
using Kulinarka.DTO;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RecipesController:BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly ILoginService loginService;
        private readonly ISessionService sessionService;
        private readonly IMapper mapper;
        private readonly PostRecipeService postRecipeService;
        public RecipesController(IRecipeService recipeService,ILoginService loginService,ISessionService sessionService,IMapper mapper,PostRecipeService postRecipeService)
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
            var result= await recipeService.GetByIdAsync(id);
            return HandleResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] PostRecipeDTO recipe)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await postRecipeService.AddRecipeAsync(loginResult.Data, recipe);
            return HandleResponse(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] Recipe recipe)
        {
            if (id != recipe.Id)
                return BadRequest();
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
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


    }
}
