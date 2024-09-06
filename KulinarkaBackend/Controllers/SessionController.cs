using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class SessionController:BaseController
    {
        private readonly ILoginService loginService;
        public SessionController(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LoginRequest loginRequest)
        {
            var loginResult = await loginService.GetSessionAsync();
            if (loginResult.IsSuccess)
                return BadRequest("Already loggedIn");
            var result = await loginService.LogInAsync(loginRequest.Username, loginRequest.Password, loginRequest.RememberMe);
            return HandleResponse(result);
        }
        [HttpDelete]
        public async Task<IActionResult> LogOut()
        {
            var result = await loginService.LogOutAsync();
            return HandleResponse(result);
        }
        [HttpGet]
        public async Task<IActionResult>GetSession()
        {
            var result = await loginService.GetSessionAsync();
            return HandleResponse(result);
        }
    }
}
