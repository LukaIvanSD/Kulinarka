﻿using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

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
        public async Task<Response<List<RecipeTag>>> AddAsync(int recipeId, List<Tag> tags)
        {
            List<RecipeTag> recipeTags = new List<RecipeTag>();
            foreach (Tag tag in tags)
            {
                var tagResult = await tagService.GetTag(tag);
                if (!tagResult.IsSuccess)
                    return Response<List<RecipeTag>>.Failure(tagResult.ErrorMessage, tagResult.StatusCode);
                RecipeTag recipeTag = new RecipeTag(recipeId, tagResult.Data.Id);
                recipeTags.Add(recipeTag);
                SaveTag(recipeTag);
            }
            return Response<List<RecipeTag>>.Success(recipeTags, StatusCode.Created);
        }
        private async void SaveTag(RecipeTag recipeTag)
        {
            await recipeTagRepository.CreateAsync(recipeTag,false);
        }
    }
}
