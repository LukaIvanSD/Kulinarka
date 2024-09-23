using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Profiles
{
    public class UserAchievementProfile : Profile
    {
        public UserAchievementProfile()
        {
            CreateMap<Achievement, AchievementDTO>()
            .ForMember(dest => dest.IconBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Icon)))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Text));

            CreateMap<UserAchievement, UserAchievementDTO>()
            .ForMember(dest => dest.AquiredDate, opt => opt.MapFrom(src => src.AquiredDate))
            .ForMember(dest => dest.PointsCollected, opt => opt.MapFrom(src => src.PointsCollected));

            CreateMap<UserAchievementDTO, UserAchievement>();
        }
    }
}
