using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Services;
using Microsoft.AspNetCore.Mvc;
using Kulinarka.Models.Responses;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RecipesController:BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly ILoginService loginService;
        private readonly ISessionService sessionService;
        public RecipesController(IRecipeService recipeService,ILoginService loginService,ISessionService sessionService)
        {
            this.recipeService = recipeService;
            this.loginService = loginService;
            this.sessionService = sessionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var result = await recipeService.GetRecipesAsync();
            return HandleResponse(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var result= await recipeService.GetRecipeAsync(id);
            return HandleResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] Recipe recipe)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await recipeService.AddRecipeAsync(loginResult.Data, recipe);
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
            var result = await recipeService.UpdateRecipeAsync(loginResult.Data, recipe);
            return HandleResponse(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await recipeService.DeleteRecipeAsync(loginResult.Data, id);
            return HandleResponse(result);
        }

    }
}
