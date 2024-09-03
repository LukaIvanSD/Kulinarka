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
    public class UserController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly AppDbContext _context;
        public UserController(AppDbContext context, ILoginService loginService
        {
            _context = context;
            this._loginService = loginService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();
            return new JsonResult(users);

        }



    }
}
