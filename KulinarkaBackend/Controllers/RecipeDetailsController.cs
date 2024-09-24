using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RecipeDetailsController :BaseController
    {
        private readonly IRecipeDetailsService recipeDetailsService;
        private readonly ILoginService loginService;
        public RecipeDetailsController(IRecipeDetailsService recipeDetailsService,ILoginService loginService)
        {
            this.recipeDetailsService = recipeDetailsService;
            this.loginService = loginService;
        }
        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetRecipeDetails(int recipeId)
        {
            var recipeDetails = await recipeDetailsService.GetRecipeDetails(recipeId);
            return HandleResponse(recipeDetails);
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateRecipeDetails([FromBody]RecipeDetailsDTO recipeDetailsDTO)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return Unauthorized("Not loggedIn");
            var recipeDetails = await recipeDetailsService.UpdateRecipeDetailsAsync(loginResult.Data,recipeDetailsDTO);
            return HandleResponse(recipeDetails);
        }

    }
}
