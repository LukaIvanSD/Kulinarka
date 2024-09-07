using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Profiles
{
    public class UserAchievementProfile : Profile
    {
        public UserAchievementProfile()
        {
            CreateMap<UserAchievement, UserAchievementDTO>();
            CreateMap<UserAchievementDTO, UserAchievement>();
        }
    }
}
