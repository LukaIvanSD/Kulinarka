namespace Kulinarka.DTO
{
    public class UserPreparedRecipeImageResponse
    {
        public int Id { get; set; }
        public string ImageBase64 { get; set; }
        public int RecipeId { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
