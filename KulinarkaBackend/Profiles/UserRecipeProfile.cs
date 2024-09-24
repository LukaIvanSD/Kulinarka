using AutoMapper;
using Kulinarka.DTO;
using Kulinarka.Models;

namespace Kulinarka.Profiles
{
    public class UserRecipeProfile : Profile
    {
        public UserRecipeProfile() {
            CreateMap<Recipe, RecipeInfoDTO>().ForMember(dest => dest.PictureBase64, opt => opt.MapFrom(src => Convert.ToBase64String(src.Picture)));
            CreateMap<RecipeIngredient, RecipeIngredientDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.MeasurementUnit, opt => opt.MapFrom(src => src.MeasurementUnit));
            CreateMap<RecipeTag, Tag>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Tag.Name))
                .ForMember(dest => dest.TagType, opt => opt.MapFrom(src => src.Tag.TagType));
            CreateMap<Recipe, UserRecipeDTO>()
          .ForMember(dest => dest.Recipe, opt => opt.MapFrom(src => src)) // Mapiraj Recipe na RecipeInfoDTO
          .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients)) // Mapiraj Ingredients
          .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags)) // Mapiraj Tagove
          .ForMember(dest => dest.IsPromoted, opt => opt.MapFrom((src, dest, _, context) =>
              (bool)context.Items["IsPromoted"])); // Mapiraj IsPromoted iz konteksta
        }
    
    }
}
