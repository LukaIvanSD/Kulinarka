namespace Kulinarka.DTO
{
    public class PreparedRecipeImageDTO
    {
        public int RecipeId { get; set; }
        public byte[]? Image { get; set; }
        public bool IsValid()
        {
            return Image == null || Image.Length == 0;
        }
    }
}
