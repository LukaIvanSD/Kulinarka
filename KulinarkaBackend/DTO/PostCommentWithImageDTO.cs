

namespace Kulinarka.DTO
{
    public class PostCommentWithImageDTO
    {
        public int RecipeId { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public byte[]? Image { get; set; }
        public bool IsInvalid()
        {
            return Image == null || Image.Length == 0;
        }

        internal CreateCommentDTO ToCreateCommentDTO()
        {
            return new CreateCommentDTO
            {
                Header = Header,
                Text = Text,
                RecipeId = RecipeId
            };
        }

        internal PreparedRecipeImageDTO ToPreparedRecipeImageDTO()
        {
            return new PreparedRecipeImageDTO
            {
                RecipeId = RecipeId,
                Image = Image
            };
        }

    }
}
