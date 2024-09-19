using Kulinarka.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kulinarka.DTO
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public byte[]? Picture { get; set; }

        public byte[]? VideoData { get; set; }
        public string? ContentType { get; set; }
        public TimeSpan Duration { get; set; }
        public Difficulty Difficulty { get; set; }
        public int NumberOfPeople { get; set; }
        public string? ChefsAdvice { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<RecipeIngredient>? Ingredients { get; set; }
        public virtual ICollection<RecipeTag>? Tags { get; set; }
        public virtual ICollection<PreparationStep>? PreparationSteps { get; set; }
        public bool IsPromoted { get; set; }
        public RecipeDTO() { }
        public RecipeDTO(Recipe recipe)
        {
            Id = recipe.Id;
            UserId = recipe.UserId;
            Name = recipe.Name;
            Description = recipe.Description;
            Picture = recipe.Picture;
            VideoData = recipe.VideoData;
            ContentType = recipe.ContentType;
            Duration = recipe.Duration;
            Difficulty = recipe.Difficulty;
            NumberOfPeople = recipe.NumberOfPeople;
            ChefsAdvice = recipe.ChefsAdvice;
            CreationDate = recipe.CreationDate;
            User = recipe.User;
            Ingredients = recipe.Ingredients;
            Tags = recipe.Tags;
            PreparationSteps = recipe.PreparationSteps;
            IsPromoted = recipe.IsPromoted();
        }
    }
}
