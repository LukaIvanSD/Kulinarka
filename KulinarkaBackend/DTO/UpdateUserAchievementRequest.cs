using Kulinarka.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.DTO
{
    public class UpdateUserAchievementRequest
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public RequirementType requirementType { get; set; }
    }
}
