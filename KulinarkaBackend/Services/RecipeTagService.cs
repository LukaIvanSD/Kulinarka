using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class RecipeTagService : IRecipeTagService
    {
        private readonly IRecipeTagRepository recipeTagRepository;
        private readonly ITagService tagService;
        public RecipeTagService(IRecipeTagRepository recipeTagRepository, ITagService tagService)
        {
            this.recipeTagRepository = recipeTagRepository;
            this.tagService = tagService;
        }
        public async Task<Response<List<RecipeTag>>> AddAsync(int recipeId, List<Tag> tags,bool saveChanges=true)
        {
            List<RecipeTag> recipeTags = new List<RecipeTag>();
            foreach (Tag tag in tags)
            {
                var tagResult = await tagService.GetByIdAsync(tag.Id);
                if (!tagResult.IsSuccess)
                    return Response<List<RecipeTag>>.Failure(tagResult.ErrorMessage, tagResult.StatusCode);
                RecipeTag recipeTag = new RecipeTag(recipeId, tagResult.Data.Id);
                recipeTags.Add(recipeTag);
                SaveTag(recipeTag,saveChanges);
            }
            return Response<List<RecipeTag>>.Success(recipeTags, StatusCode.Created);
        }

        public async Task<Response<List<RecipeTag>>> GetByRecipeIdAsync(int recipeId)
        {
            return await recipeTagRepository.GetByRecipeIdEagerAsync(recipeId);
        }

        public async Task<Response<List<RecipeTag>>> UpdateAsync(List<RecipeTag> oldRecipeTags, List<Tag> newTags, bool saveChanges=true)
        {
            int oldTagsCount = oldRecipeTags.Count;
            int newTagsCount = newTags.Count;
            int minCount=Math.Min(oldTagsCount, newTagsCount);
            var updateResult = await UpdateExistingTags(oldRecipeTags.GetRange(0, minCount), newTags.GetRange(0,minCount),saveChanges);
            if (!updateResult.IsSuccess)
                return Response<List<RecipeTag>>.Failure(updateResult.ErrorMessage, updateResult.StatusCode);
            if (oldTagsCount<=minCount)
                return await AddAsync(oldRecipeTags[0].RecipeId, newTags.GetRange(minCount, newTagsCount - minCount),saveChanges);
            else
                return await DeleteTags(oldRecipeTags.GetRange(minCount, oldTagsCount - minCount));
        }

        private async Task<Response<List<RecipeTag>>> UpdateExistingTags(List<RecipeTag> recipeTagsToUpdate, List<Tag> newTags, bool saveChanges=true)
        {
            for (int i = 0; i < recipeTagsToUpdate.Count; i++)
            {
                var tagResult = await tagService.GetByIdAsync(newTags[i].Id);
                if (!tagResult.IsSuccess)
                    return Response<List<RecipeTag>>.Failure(tagResult.ErrorMessage, tagResult.StatusCode);
                recipeTagsToUpdate[i].TagId = tagResult.Data.Id;
                var updateResult = await recipeTagRepository.UpdateAsync(recipeTagsToUpdate[i].Id, recipeTagsToUpdate[i], saveChanges);
                if (!updateResult.IsSuccess)
                    return Response<List<RecipeTag>>.Failure(updateResult.ErrorMessage, updateResult.StatusCode);
            }
            return Response<List<RecipeTag>>.Success(recipeTagsToUpdate, StatusCode.OK);
        }

        public async Task<Response<List<RecipeTag>>> DeleteTags(List<RecipeTag> recipeTags,bool saveChanges=true)
        {
            foreach (RecipeTag recipeTag in recipeTags)
            {
                var deleteResult=await recipeTagRepository.DeleteAsync(recipeTag.Id,saveChanges);
                if (!deleteResult.IsSuccess)
                    return Response<List<RecipeTag>>.Failure(deleteResult.ErrorMessage, deleteResult.StatusCode);
            }
            return Response<List<RecipeTag>>.Success(recipeTags, StatusCode.OK);
        }

        private async void SaveTag(RecipeTag recipeTag,bool saveChanges=true)
        {
            await recipeTagRepository.CreateAsync(recipeTag, saveChanges);
        }
    }
}
