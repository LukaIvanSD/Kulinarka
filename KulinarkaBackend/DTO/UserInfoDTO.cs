using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class UserInfoDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateOnly Birthday { get; set; }
        public string? Location { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Bio { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string PictureBase64 { get; set; }
    }
}
