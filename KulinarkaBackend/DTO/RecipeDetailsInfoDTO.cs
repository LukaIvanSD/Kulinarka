using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class RecipeDetailsInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public string PictureBase64 { get; set; }
        public TimeSpan Duration { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime CreationDate { get; set; }
        public string? VideoBase64 { get; set; }
        public string? ContentType { get; set; }
        public string ChefsAdvice { get; set; }
        public RecipeDetailsInfoDTO()
        {
        }

    }
}
