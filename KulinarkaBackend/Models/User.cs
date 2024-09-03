using System.ComponentModel.DataAnnotations;

namespace Kulinarka.Models
{
    public enum Gender
    {
        Male = 0,
        Female = 1
    }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateOnly Birthday { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public DateTime DateOfCreation { get; set; }

        public string Bio { get; set; }

        public byte[] Picture { get; set; }
    }
}
