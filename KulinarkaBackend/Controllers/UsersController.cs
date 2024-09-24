using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kulinarka.Models;
using Newtonsoft.Json;
using Kulinarka.Interfaces;
using System.Diagnostics;
using Kulinarka.Controllers;
using Kulinarka.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILoginService loginService;
        public UsersController(IUserService userService, ILoginService loginService)
        {
            _userService = userService;
            this.loginService = loginService;
        }
        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userService.GetUsersAsync();
            return HandleResponse(result);

        }
        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return HandleResponse(result);
        }
        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile picture, [FromForm] User user)
        {
            if (picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await picture.CopyToAsync(memoryStream);
                    user.Picture = memoryStream.ToArray();
                }
            }
            var result = await _userService.RegisterUserAsync(user);
            return HandleResponse(result);
        }
        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("Id missmatch");
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await _userService.UpdateUserAsync(loginResult.Data, user);
            return HandleResponse(result);

        }
        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return HandleResponse(result);
        }




    }
}
