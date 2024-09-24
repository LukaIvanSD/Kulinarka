using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using Kulinarka.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserAchievementController :BaseController
    {
        public readonly IUserAchievementService _userAchievementService;
        private readonly ILoginService loginService;
        private readonly IMapper mapper;
        private readonly IUserManagementService userManagementService;
        public UserAchievementController(IUserAchievementService userAchievementService,ILoginService loginService, IMapper mapper,IUserManagementService userManagementService)
        {
            _userAchievementService = userAchievementService;
            this.loginService = loginService;
            this.mapper = mapper;
            this.userManagementService = userManagementService;
        }
        [HttpPatch("addProgress")]
        public async Task<IActionResult> AddProgress([FromBody] UpdateUserAchievementRequest request)
        {
             var result = await userManagementService.AddProgress(request.userId,request.requirementType);
             return HandleResponse(result);
        }
        [HttpPatch("removeProgress")]
        public async Task<IActionResult> RemoveProgress([FromBody] UpdateUserAchievementRequest request)
        {
            var result = await userManagementService.RemoveProgress(request.userId,request.requirementType);
            return HandleResponse(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetUserAchievements()
        {
            var loginResult = await loginService.GetSessionAsync();
            if (!loginResult.IsSuccess)
                return StatusCode((int)loginResult.StatusCode, loginResult.ErrorMessage);
            var result = await _userAchievementService.GetUserAchievementsEagerAsync(loginResult.Data.Id);
            if (!result.IsSuccess)
                return StatusCode((int)result.StatusCode, result.ErrorMessage);
            return Ok(mapper.Map<List<UserAchievementDTO>>(result.Data));
        }

    }
}
