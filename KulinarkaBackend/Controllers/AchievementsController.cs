using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace Kulinarka.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AchievementsController : BaseController
    {
        private readonly IAchievementService achievementService;
        public AchievementsController(IAchievementService achievementService) { 
        this.achievementService = achievementService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAchievements()
        {
            var result = await achievementService.GetAchievementsAsync();
            return HandleResponse(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAchievement(int id)
        {
            var result = await achievementService.GetAchievementAsync(id);
            return HandleResponse(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAchievement([FromBody] Achievement achievement)
        {
            var result = await achievementService.CreateAchievement(achievement);
            return HandleResponse(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAchievement(int id, [FromBody] Achievement achievement)
        {
            if (id != achievement.Id)
                return BadRequest();
            var result = await achievementService.UpdateAchievementAsync(achievement);
            return HandleResponse(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAchievement(int id)
        {
            var result = await achievementService.DeleteAchievementAsync(id);
            return HandleResponse(result);
        }

    }
}
