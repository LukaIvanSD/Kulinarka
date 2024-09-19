using Kulinarka.DTO;
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
        public async Task<Response<List<PreparationStep>>> AddAsync(int recipeId, List<PreparationStepDTO> preparationStepsDTO)
        {
            var validationResult = ValidatePreparationSteps(preparationStepsDTO);
            if (!validationResult.IsSuccess)
                return Response<List<PreparationStep>>.Failure(validationResult.ErrorMessage, StatusCode.BadRequest);
            List<PreparationStep>preparationSteps = new List<PreparationStep>();
            foreach (PreparationStepDTO step in preparationStepsDTO)
            {
                PreparationStep preparationStep = CreatePreparationStep(step,recipeId);
                var result = await preparationStepRepository.CreateAsync(preparationStep, false);
                if (!result.IsSuccess)
                    return Response<List<PreparationStep>>.Failure(result.ErrorMessage, result.StatusCode);
                preparationSteps.Add(result.Data);
            }

            return Response<List<PreparationStep>>.Success(preparationSteps, StatusCode.Created);
        }

        private PreparationStep CreatePreparationStep(PreparationStepDTO step, int recipeId)
        {
            return new PreparationStep(step.Description, step.SequenceNumber, recipeId);
        }

        private Response<PreparationStepDTO> ValidatePreparationSteps(List<PreparationStepDTO> preparationSteps)
        {
            int totalSteps = preparationSteps.Count;
            HashSet<int> existingSequences = new HashSet<int>();

            foreach (PreparationStepDTO step in preparationSteps)
            {
                if (existingSequences.Contains(step.SequenceNumber))
                    return Response<PreparationStepDTO>.Failure("Duplicate sequence number",StatusCode.BadRequest);

                if (!IsSequenceValid(totalSteps,step.SequenceNumber))
                    return Response<PreparationStepDTO>.Failure("Invalid sequence number", StatusCode.BadRequest);

                existingSequences.Add(step.SequenceNumber);
            }
            return Response<PreparationStepDTO>.Success(null, StatusCode.OK);
        }

        private bool IsSequenceValid(int totalSteps,int sequenceNumber)
        {
            return sequenceNumber > 0 && sequenceNumber <= totalSteps;
        }
    }
}
