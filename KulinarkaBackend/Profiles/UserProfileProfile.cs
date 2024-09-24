using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Profiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<User, UserInfoDTO>()
                .ForMember(dest => dest.PictureBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Picture)));
            CreateMap<User,UserStatisticDTO>()
                .ForMember(dest => dest.NumberOfFollowers, opt => opt.MapFrom(src => src.UserStatistic.Followers))
                .ForMember(dest => dest.NumberOfLikes, opt => opt.MapFrom(src => src.UserStatistic.Likes))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.UserStatistic.AverageRating))
                .ForMember(dest => dest.NumberOfFavorites, opt => opt.MapFrom(src => src.UserStatistic.Favorites))
                .ForMember(dest => dest.NumberOfRecipes, opt => opt.MapFrom((src, dest, destMembers, context) => context.Items["RecipeCount"]));
            CreateMap<User, TitleDTO>()
                .ForMember(dest => dest.CurrentTitleRequirement, opt => opt.MapFrom(src => src.UserTitle.CurrentTitle.AchievementsRequired))
                .ForMember(dest => dest.NextTitleRequirement, opt => opt.MapFrom(src => src.UserTitle.NextTitle.AchievementsRequired))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserTitle.CurrentTitle.TitleType))
                .ForMember(dest => dest.NextTitleName, opt => opt.MapFrom(src => src.UserTitle.NextTitle.TitleType))
                .ForMember(dest => dest.AquireDate, opt => opt.MapFrom(src => src.UserTitle.AquiredDate))
                .ForMember(dest => dest.CompletedAchievements, opt => opt.MapFrom((src, dest, destMembers, context) => context.Items["AchievementsCompleted"]));
            CreateMap<PromotionReward, PromotionRewardDTO>();


        }
    }
}
