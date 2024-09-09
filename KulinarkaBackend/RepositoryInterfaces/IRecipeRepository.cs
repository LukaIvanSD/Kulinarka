using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IRecipeRepository : IRepository<Recipe>
    {
        Task<Response<List<Recipe>>> GetAllAsync();
        Task<Response<Recipe>> GetByIdAsync(int id);
        Task<Response<Recipe>> CreateAsync(Recipe recipe, bool saveChanges = true);
        Task<Response<Recipe>> UpdateAsync(int id, Recipe recipe, bool saveChanges = true);
        Task<Response<Recipe>> DeleteAsync(int id, bool saveChanges = true);
    }
}
