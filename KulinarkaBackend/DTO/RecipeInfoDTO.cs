using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class RecipeInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public string PictureBase64 { get; set; }
        public TimeSpan Duration { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime CreationDate { get; set; }



        public RecipeInfoDTO() { }
    }
}
