using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : BaseController
    {
        private readonly ICommentService commentService;
        private readonly ILoginService loginService;
        public CommentController(ICommentService commentService,ILoginService loginService)
        {
            this.commentService = commentService;
            this.loginService = loginService;
        }
        [HttpGet("recipe/{recipeId}")]
        public async Task<IActionResult> GetCommentsForRecipePaged(int recipeId,[FromQuery] int pageNumber,[FromQuery] int pageSize)
        {
            var result = await commentService.GetByRecipeIdPagedAsync(recipeId, pageNumber, pageSize);
            return HandleResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCommentDTO createCommentDTO)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return Unauthorized();

            var result = await commentService.CreateAsync(loginResult.Data,createCommentDTO);
            return HandleResponse(result);
        }
    }
}
