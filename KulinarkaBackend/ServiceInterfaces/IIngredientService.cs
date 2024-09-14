
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IIngredientService
    {
        Task<Response<Ingredient>> GetIngredientByName(string ingredientName);
    }
}
