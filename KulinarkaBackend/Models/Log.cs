using System.ComponentModel.DataAnnotations;

namespace Kulinarka.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Method { get; set; }
        [Required]
        [MaxLength(70)]
        public string Path { get; set; }
        [Required]
        public double Duration { get; set; }
        [MaxLength(200)]
        public string UserAgent { get; set; }
        [MaxLength(200)]
        public string Referer { get; set; }

    }
}
