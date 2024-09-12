using Kulinarka.Interfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PromotionRewardRecipeController :BaseController
    {
        private readonly IPromotionRewardRecipeService promotionRewardRecipeService;
        private readonly ILoginService loginService;
        public PromotionRewardRecipeController(IPromotionRewardRecipeService promotionRewardRecipeService, ILoginService loginService)
        {
            this.promotionRewardRecipeService = promotionRewardRecipeService;
            this.loginService = loginService;
        }
        [HttpPost("{recipeId}")]
        public async Task<IActionResult> PromoteRecipe(int recipeId) {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await promotionRewardRecipeService.PromoteRecipeAsync(loginResult.Data, recipeId);
            return HandleResponse(result);

        }
    }
}
