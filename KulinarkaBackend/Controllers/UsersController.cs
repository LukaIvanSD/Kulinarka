using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kulinarka.Models;
using Newtonsoft.Json;
using Kulinarka.Interfaces;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsersAsync());

        }
        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            User user = await _userService.GetUserAsync(id);
            if (user==null)
                return NotFound();
            return  Ok(user);
        }
        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User newUser=await _userService.RegisterUserAsync(user);
            if (newUser == null)
                return BadRequest();
            return Ok(newUser);
        }
        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,[FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return BadRequest("Id missmatch");

            if(!await _userService.UpdateUserAsync(user))
                return BadRequest();

            return Ok(user);

        }
        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _userService.DeleteUserAsync(id))
                return NotFound();
            return Ok();
        }




    }
}
