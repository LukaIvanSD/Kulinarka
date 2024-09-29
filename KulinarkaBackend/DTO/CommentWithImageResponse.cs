namespace Kulinarka.DTO
{
    public class CommentWithImageResponse
    {
        public CommentDTO Comment { get; set; }
        public PreparedRecipeImageResponse PreparedRecipeImage { get; set; }
        public CommentWithImageResponse(CommentDTO comment, PreparedRecipeImageResponse preparedRecipeImage)
        {
            Comment = comment;
            PreparedRecipeImage = preparedRecipeImage;
        }
    }
}
