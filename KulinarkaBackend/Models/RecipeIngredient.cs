using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kulinarka.Models
{
    public enum MeasurementUnit
    {
        Gram,
        Kilogram,
        Liter,
        Milliliter,
        Piece,
        Tablespoon,
        Teaspoon,
        Drop,
        Cup,
        Slice
    }
    public class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public double Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        [JsonIgnore]
        public virtual Recipe? Recipe { get; set; }
        [JsonIgnore]
        public virtual Ingredient? Ingredient { get; set; }
        public RecipeIngredient(int recipeId, int ingredientId, double amount, MeasurementUnit measurementUnit)
        {
            RecipeId = recipeId;
            IngredientId = ingredientId;
            Amount = amount;
            MeasurementUnit = measurementUnit;
        }
    }
}
