using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IPreparationStepService
    {
        Task<Response<List<PreparationStep>>> AddAsync(int recipeId, List<PreparationStep> preparationSteps);
    }
}
