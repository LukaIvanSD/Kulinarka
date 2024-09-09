using Kulinarka.DTO;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UserAchievementController :BaseController
    {
        public readonly IUserAchievementService _userAchievementService;
        public readonly IUserRepository userAchievementRepository;
        public UserAchievementController(IUserAchievementService userAchievementService,IUserRepository userAchievementRepository)
        {
            _userAchievementService = userAchievementService;
            this.userAchievementRepository = userAchievementRepository;
        }
        [HttpPatch("/addProgress")]
        public async Task<IActionResult> AddProgress([FromBody] UpdateUserAchievementRequest request)
        {
             var result = await _userAchievementService.AddProgress(request.userId,request.requirementType);
             return HandleResponse(result);
        }
        [HttpPatch("/removeProgress")]
        public async Task<IActionResult> RemoveProgress([FromBody] UpdateUserAchievementRequest request)
        {
            var result = await _userAchievementService.RemoveProgress(request.userId,request.requirementType);
            return HandleResponse(result);
        }

    }
}
