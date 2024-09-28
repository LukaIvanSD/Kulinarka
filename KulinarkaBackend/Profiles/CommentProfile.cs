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
        }
    }
}
