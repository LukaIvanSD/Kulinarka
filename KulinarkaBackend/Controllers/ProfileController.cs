using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.ServiceInterfaces;
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
    }
}
