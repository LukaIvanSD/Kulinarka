using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.ServiceInterfaces;
using Kulinarka.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ProfileController : BaseController
    {
        private IProfileService profileService;
        private ILoginService loginService;
        public ProfileController(IProfileService profileService,ILoginService loginService)
        {
            this.loginService = loginService;
            this.profileService = profileService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var loginResult =await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return Unauthorized(loginResult.ErrorMessage);
            var profileResult =await profileService.GetProfileDTO(loginResult.Data.Id); 
            return HandleResponse(profileResult);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordChangeRequst passwordChangeRequst)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await profileService.UpdatePasswordAsync(loginResult.Data, passwordChangeRequst);
            return HandleResponse(result);
        }
        [HttpPut("ChangeInfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UserInfoDTO newUserInfo)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await profileService.UpdateUserInfoAsync(loginResult.Data, newUserInfo);
            return HandleResponse(result);

        }
        [HttpPut("ChangePicture")]
        public async Task<IActionResult> UpdatePicture(IFormFile picture)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            if (picture == null || picture.Length == 0)
                return BadRequest("Invalid picture file.");
            using (var memoryStream = new MemoryStream())
            {
                await picture.CopyToAsync(memoryStream);
                byte[] pictureBytes = memoryStream.ToArray();
                var result = await profileService.UpdatePictureAsync(loginResult.Data, pictureBytes);
                return HandleResponse(result);
            }
        }
    }
}
