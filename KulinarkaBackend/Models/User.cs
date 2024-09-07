using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Converters;

namespace Kulinarka.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="First name is required") ]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Last name is required")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Gender is required")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }

        [Required(ErrorMessage ="Birthday is required")]
        public DateOnly Birthday { get; set; }

        [MaxLength(100)]
        public string? Location { get; set; }

        [Required(ErrorMessage ="E-mail is required")]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Username is required")]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [MaxLength(100)]
        public string Password { get; set; }

        public DateTime DateOfCreation { get; set; }
        public string? Bio { get; set; }

        public byte[]? Picture { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserAchievement> UserAchievements { get; set; }
    }
}
