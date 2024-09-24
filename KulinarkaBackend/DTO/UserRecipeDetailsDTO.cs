namespace Kulinarka.DTO
{
    public class UserRecipeDetailsDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string PictureBase64 { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
