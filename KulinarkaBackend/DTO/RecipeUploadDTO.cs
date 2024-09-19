namespace Kulinarka.DTO
{
    public class RecipeUploadDTO
    {
        public string Recipe { get; set; }
        public string Ingredients { get; set; }
        public string PreparationSteps { get; set; }
        public string Tags { get; set; }

        // Slika i video fajlovi
        public IFormFile Picture { get; set; }
        public IFormFile? Video { get; set; }
        public string? ContentType { get; set; }
    }
}
