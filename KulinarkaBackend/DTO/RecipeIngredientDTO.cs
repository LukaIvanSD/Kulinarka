
using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class RecipeIngredientDTO
    {
        public string Name { get; set; }
        public float Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public RecipeIngredientDTO() { }
    }
}
