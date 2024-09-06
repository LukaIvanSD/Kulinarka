using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kulinarka.Models
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required(ErrorMessage ="Name of recipe is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
        //[Required(ErrorMessage ="Picture of recipe is required")]
        public byte[]? Picture { get; set; }

        public byte[]? VideoData { get; set; }
        [MaxLength(100)]
        public string? ContentType { get; set; }

        [Required(ErrorMessage ="Duration is required")]
        public TimeSpan Duration { get; set; }
        [Required(ErrorMessage = "Difficulty is required")]
        public Difficulty Difficulty { get; set; }
        [Required(ErrorMessage ="Number of people is required")]
        public int NumberOfPeople { get; set; }
        public string? ChefsAdvice { get; set; }
        public DateTime CreationDate { get; set; }
        public User? User { get; set; }


    }
}
