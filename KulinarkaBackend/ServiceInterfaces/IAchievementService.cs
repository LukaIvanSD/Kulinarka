using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IAchievementService
    {
        Task<Response<Achievement>> AddAchievementAsync(Achievement achievement,bool saveChanges=true);
        Task<Response<Achievement>> DeleteAchievementAsync(int id, bool saveChanges = true);
        Task<Response<List<Achievement>>> GetAchievementsAsync();
        Task<Response<Achievement>> GetAchievementAsync(int id);
        Task<Response<Achievement>> UpdateAchievementAsync(Achievement achievement, bool saveChanges = true);
        Task<Response<Achievement>> CreateAchievement(Achievement achievement);
    }
}
