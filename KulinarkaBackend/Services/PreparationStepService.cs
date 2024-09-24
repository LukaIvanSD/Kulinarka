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
        public async Task<Response<List<PreparationStep>>> AddAsync(int recipeId, List<PreparationStepDTO> preparationStepsDTO,bool saveChanges=true)
        {
            var validationResult = ValidatePreparationSteps(preparationStepsDTO);
            if (!validationResult.IsSuccess)
                return Response<List<PreparationStep>>.Failure(validationResult.ErrorMessage, StatusCode.BadRequest);
            List<PreparationStep>preparationSteps = new List<PreparationStep>();
            foreach (PreparationStepDTO step in preparationStepsDTO)
            {
                PreparationStep preparationStep = CreatePreparationStep(step,recipeId);
                var result = await preparationStepRepository.CreateAsync(preparationStep, saveChanges);
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

        //Function does not return new list of preparation steps
        public async Task<Response<List<PreparationStep>>> UpdateAsync(List<PreparationStep> oldPreparationSteps, List<PreparationStepDTO> newPreparationSteps, bool saveChanges = true)
        {
            int oldStepsCount = oldPreparationSteps.Count;
            int newStepsCount = newPreparationSteps.Count;
            int recipeId = oldPreparationSteps[0].RecipeId;
            for (int i = 0; i <Math.Max(oldStepsCount,newStepsCount); i++)
            {
                if (oldPreparationSteps[i] == null)
                    return await AddAsync(recipeId,newPreparationSteps.GetRange(i,newStepsCount-i), saveChanges);
                if (newPreparationSteps[i] == null)
                    return await DeleteAsync(oldPreparationSteps.GetRange(i,oldStepsCount-i), saveChanges);
                var updateResult = await UpdateExistingAsync(oldPreparationSteps[i],newPreparationSteps[i], saveChanges);
                if (!updateResult.IsSuccess)
                    return Response<List<PreparationStep>>.Failure(updateResult.ErrorMessage, updateResult.StatusCode);
            }
            return Response<List<PreparationStep>>.Success(oldPreparationSteps, StatusCode.OK);
        }

        public async Task<Response<PreparationStep>> UpdateExistingAsync(PreparationStep stepToUpdate, PreparationStepDTO newPreparationStep, bool saveChanges)
        {
            stepToUpdate.Description = newPreparationStep.Description;
            stepToUpdate.SequenceNum = newPreparationStep.SequenceNumber;
            return await preparationStepRepository.UpdateAsync(stepToUpdate.Id, stepToUpdate, saveChanges);
        }

        public async Task<Response<List<PreparationStep>>> DeleteAsync(List<PreparationStep> oldPreparationSteps, bool saveChanges=true)
        {
            foreach (PreparationStep step in oldPreparationSteps)
            {
                var result = await preparationStepRepository.DeleteAsync(step.Id, saveChanges);
                if (!result.IsSuccess)
                    return Response<List<PreparationStep>>.Failure(result.ErrorMessage, result.StatusCode);
            }
            return Response<List<PreparationStep>>.Success(null, StatusCode.OK);
        }

    }
}
