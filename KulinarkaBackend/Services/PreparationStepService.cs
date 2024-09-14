using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class PreparationStepService : IPreparationStepService
    {
        private readonly IPreparationStepRepository preparationStepRepository;
        public PreparationStepService(IPreparationStepRepository preparationStepRepository)
        {
            this.preparationStepRepository = preparationStepRepository;
        }
        public async Task<Response<List<PreparationStep>>> AddAsync(int recipeId, List<PreparationStep> preparationSteps)
        {
            var validationResult = ValidatePreparationSteps(preparationSteps);
            if (!validationResult.IsSuccess)
                return Response<List<PreparationStep>>.Failure(validationResult.ErrorMessage, StatusCode.BadRequest);

            foreach (PreparationStep step in preparationSteps)
            {
                step.RecipeId = recipeId;
                var result = await preparationStepRepository.CreateAsync(step, false);
                if (!result.IsSuccess)
                    return Response<List<PreparationStep>>.Failure(result.ErrorMessage, result.StatusCode);
            }

            return Response<List<PreparationStep>>.Success(preparationSteps, StatusCode.Created);
        }

        private Response<PreparationStep> ValidatePreparationSteps(List<PreparationStep> preparationSteps)
        {
            int totalSteps = preparationSteps.Count;
            HashSet<int> existingSequences = new HashSet<int>();

            foreach (PreparationStep step in preparationSteps)
            {
                if (existingSequences.Contains(step.SequenceNum))
                    return Response<PreparationStep>.Failure("Duplicate sequence number",StatusCode.BadRequest);

                if (!step.IsSequenceValid(totalSteps))
                    return Response<PreparationStep>.Failure("Invalid sequence number", StatusCode.BadRequest);

                existingSequences.Add(step.SequenceNum);
            }
            return Response<PreparationStep>.Success(null, StatusCode.OK);
        }

    }
}
