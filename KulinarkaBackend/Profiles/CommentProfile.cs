using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Profiles
{
    public class CommentProfile :Profile
    {
        public CommentProfile()
        {
            CreateMap<CreateCommentDTO, Comment>()
                .ForMember(dest=>dest.DateCreated,opt=>opt.MapFrom(src=>DateTime.Now))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom((src, dest, _, context)=>
                    context.Items["CreatorId"]
                ));
            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.Username))
                .ForMember(dest => dest.CreatorPictureBase64, opt => opt.MapFrom(src =>Convert.ToBase64String(src.Creator.Picture)))
                .ForMember(dest => dest.PostDate, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.Header , opt => opt.MapFrom(src => src.Header))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.CreatorTitle , opt=>opt.MapFrom(src=>src.Creator.UserTitle.CurrentTitle.TitleType))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        }
    }
}
