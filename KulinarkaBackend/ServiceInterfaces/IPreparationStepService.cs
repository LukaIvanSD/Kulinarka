using Kulinarka.Models;
using Kulinarka.DTO;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IPreparationStepService
    {
        Task<Response<List<PreparationStep>>> AddAsync(int recipeId, List<PreparationStepDTO> preparationSteps);
    }
}
