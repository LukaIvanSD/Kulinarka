
using Kulinarka.Models;

namespace Kulinarka.DTO
{
    public class RecipeIngredientDTO
    {
        public string IngredientName { get; set; }
        public float Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
    }
}
