namespace Kulinarka.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
        public string Header { get; set; }
        public string CreatorName { get; set; }
        public string CreatorTitle { get; set; }
        public string CreatorPictureBase64 { get; set; }
        public int CreatorId { get; set; }


    }
}
