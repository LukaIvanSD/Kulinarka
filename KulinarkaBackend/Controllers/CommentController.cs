using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.ServiceInterfaces;
using Kulinarka.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : BaseController
    {
        private readonly ICommentService commentService;
        private readonly PostCommentService postCommentService;
        private readonly ILoginService loginService;
        public CommentController(ICommentService commentService, ILoginService loginService, PostCommentService postCommentService)
        {
            this.commentService = commentService;
            this.loginService = loginService;
            this.postCommentService = postCommentService;
        }
        [HttpGet("recipe/{recipeId}")]
        public async Task<IActionResult> GetCommentsForRecipePaged(int recipeId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
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

            var result = await postCommentService.PostCommentAsync(loginResult.Data, createCommentDTO);
            return HandleResponse(result);
        }
        [HttpPost("picture")]
        public async Task<IActionResult> CreateWithPictureAsync(IFormFile image, [FromForm] PostCommentWithImageDTO postCommentWithImageDTO)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return Unauthorized();

            var imageResult = await UploadFilesService.UploadFileAsync(image);
            if (!imageResult.IsSuccess)
                return HandleResponse(imageResult);

            postCommentWithImageDTO.Image = imageResult.Data;

            var result = await postCommentService.PostCommentWithPictureAsync(loginResult.Data, postCommentWithImageDTO);
            return HandleResponse(result);
        }
    }
}
