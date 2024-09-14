using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        Task<Response<Ingredient>>GetByNameAsync(string ingredientName);
    }
}
