using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Profiles
{
    public class RecipeDetailsProfile :Profile
    {
        public RecipeDetailsProfile()
        { 
            CreateMap<Recipe, RecipeDetailsInfoDTO>()
                .ForMember(dest => dest.PictureBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Picture)))
                .ForMember(dest => dest.VideoBase64, opt => opt.MapFrom(src => src.VideoData == null ? null : Convert.ToBase64String(src.VideoData)));
            CreateMap<RecipeDetailsInfoDTO,Recipe>()
                .ForMember(dest => dest.Picture, opt => opt.Ignore())
                .ForMember(dest => dest.VideoData, opt => opt.Ignore())
                .ForMember(dest => dest.ContentType, opt => opt.Ignore())
                .ForMember(dest => dest.Id,opt=>opt.Ignore())
                .ForMember(dest => dest.UserId,opt=>opt.Ignore());
            CreateMap<PreparationStepDTO, PreparationStep>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore());

            CreateMap<Comment,CommentDTO>()
                .ForMember(dest=>dest.PostDate , opt=>opt.MapFrom(src=>src.DateCreated))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.Username))
                .ForMember(dest => dest.CreatorPictureBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Creator.Picture)))
                .ForMember(dest => dest.CreatorTitle, opt => opt.MapFrom(src => src.Creator.UserTitle.CurrentTitle.TitleType))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Header, opt => opt.MapFrom(src => src.Header))
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator.Id));

            CreateMap<PreparedRecipeImage,PreparedRecipeImageResponse>()
                .ForMember(dest => dest.ImageBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Image)))
                .ForMember(dest => dest.CreatorPictureBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Creator.Picture)))
                .ForMember(dest => dest.CreatorUsername, opt => opt.MapFrom(src => src. Creator.Username))
                .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => src.DateUploaded))
                .ForMember(dest => dest.CreatorId ,  opt=>opt.MapFrom(src=>src.Creator.Id));


            CreateMap<User, UserRecipeDetailsDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.UserTitle.CurrentTitle.TitleType))
                .ForMember(dest => dest.PictureBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Picture)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateOfCreation, opt => opt.MapFrom(src => src.DateOfCreation));
            CreateMap<RecipeIngredient,RecipeIngredientDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.MeasurementUnit, opt => opt.MapFrom(src => src.MeasurementUnit));
            CreateMap<PreparationStep, PreparationStepDTO>()
                .ForMember(dest => dest.SequenceNumber, opt => opt.MapFrom(src => src.SequenceNum))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<Recipe, RecipeDetailsDTO>()
                .ForMember(dest => dest.Recipe, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(rt => rt.Tag).ToList()))
                .ForMember(dest => dest.Steps, opt => opt.MapFrom(src => src.PreparationSteps))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.IsPromoted, opt => opt.MapFrom(src => src.IsPromoted()))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src=>src.Comments));
        }
    }
}
