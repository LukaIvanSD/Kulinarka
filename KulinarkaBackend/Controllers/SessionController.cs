using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class SessionController:ControllerBase
    {
        private readonly ILoginService loginService;
        public SessionController(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid )
                return BadRequest();
            if(await loginService.IsLoggedInAsync())
                return Ok("Already logged in");

            if (await loginService.LogInAsync(loginRequest.Username, loginRequest.Password, loginRequest.RememberMe) == null)
                return Unauthorized();
            return Ok("Login successfull");
        }
        [HttpDelete]
        public async Task<IActionResult> LogOut()
        {
            if(!await loginService.LogOutAsync())
                return Ok("Already logged out");
            return Ok("Logged out");
        }
        [HttpGet]
        public async Task<IActionResult>GetSession()
        {
            User user = await loginService.GetSessionAsync();
            if (user == null)
                return NotFound("Not logged in");
            return Ok(user);
        }
    }
}
