using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Profiles
{
    public class PreparedRecipeImageProfile :Profile
    {
        public PreparedRecipeImageProfile()
        {
            CreateMap<PreparedRecipeImage, UserPreparedRecipeImageResponse>()
                .ForMember(dest => dest.ImageBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.RecipeId))
                .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => src.DateUploaded));
            CreateMap<PreparedRecipeImage, PreparedRecipeImageResponse>()
                .ForMember(dest => dest.ImageBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
                .ForMember(dest => dest.CreatorPictureBase64, opt => opt.MapFrom(src => src.Creator.Picture))
                .ForMember(dest => dest.CreatorUsername, opt => opt.MapFrom(src => src.Creator.Username))
                .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => src.DateUploaded))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.CreatorId));
        }
    }
}
