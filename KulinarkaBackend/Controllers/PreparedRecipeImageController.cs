﻿using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.ServiceInterfaces;
using Kulinarka.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PreparedRecipeImageController : BaseController
    {
        private readonly IPreparedRecipeImageService preparedRecipeImageService;
        private readonly ILoginService loginService;
        public PreparedRecipeImageController(IPreparedRecipeImageService preparedRecipeImageService,ILoginService loginService) {
            this.loginService = loginService;
            this.preparedRecipeImageService = preparedRecipeImageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetByUser([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var loginResult =await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return Unauthorized();

            var response = await preparedRecipeImageService.GetByUserIdAsync(loginResult.Data.Id, pageNumber, pageSize);
            return HandleResponse(response);
        }

        [HttpGet("{recipeId}")]
        public async Task<IActionResult> GetByRecipeId(int recipeId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var response = await preparedRecipeImageService.GetByRecipeIdAsync(recipeId, pageNumber, pageSize);
            return HandleResponse(response);
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile image, [FromForm] PreparedRecipeImageDTO preparedRecipeImageDTO)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return Unauthorized();

            if (image == null || image.Length == 0)
                return BadRequest("Image is required");

          preparedRecipeImageDTO.Image= (await UploadFilesService.UploadFileAsync(image)).Data;

            var response = await preparedRecipeImageService.UploadImage(loginResult.Data, preparedRecipeImageDTO);
            return HandleResponse(response);
        }
        [HttpGet("check/{recipeId}")]
        public async Task<IActionResult> HasUploadedImageForRecipe(int recipeId)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return Unauthorized();

            var response = await preparedRecipeImageService.HasUploadedImageForRecipe(loginResult.Data.Id, recipeId);
            return HandleResponse(response);
        }
    }
}
