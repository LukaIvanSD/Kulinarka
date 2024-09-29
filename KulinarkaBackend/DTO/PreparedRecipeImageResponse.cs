namespace Kulinarka.DTO
{
    public class PreparedRecipeImageResponse
    {
        public int CreatorId { get; set; }
        public string ImageBase64 { get; set; }
        public string CreatorUsername { get; set; }
        public DateTime DateUploaded { get; set; }
        public string CreatorPictureBase64 { get; set; }
    }
}
