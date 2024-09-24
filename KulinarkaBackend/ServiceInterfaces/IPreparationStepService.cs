using Kulinarka.Models;
using Kulinarka.DTO;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IPreparationStepService
    {
        Task<Response<List<PreparationStep>>> AddAsync(int recipeId, List<PreparationStepDTO> preparationSteps,bool saveChanges=true);
        Task<Response<List<PreparationStep>>> UpdateAsync(List<PreparationStep> oldPreparationSteps, List<PreparationStepDTO> newPreparationSteps, bool saveChanges=true);
        Task<Response<List<PreparationStep>>> DeleteAsync(List<PreparationStep> oldPreparationSteps, bool saveChanges = true);


    }
}
