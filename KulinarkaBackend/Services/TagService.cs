using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;

namespace Kulinarka.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }
        public async Task<Response<Tag>> GetByIdAsync(int id)
        {
            return await tagRepository.GetByIdAsync(id);
        }

        public async Task<Response<Tag>> GetTag(Tag tag)
        {
            if (tag.TagType == TagType.PreparationTime)
            {
                var tagResult = await GetOrCreatePreparationTimeTag(tag.Name);
                return tagResult;
            }
            else
            {
                var tagResult = await GetByIdAsync(tag.Id);
                return tagResult;
            }
        }

        private async Task<Response<Tag>> GetOrCreatePreparationTimeTag(string name)
        {
            var tagResult = await tagRepository.GetByTypeAndNameAsync(TagType.PreparationTime, name);
            if (!tagResult.IsSuccess)
                return tagResult;
            if (tagResult.Data != null)
                return tagResult;
            var tag = await tagRepository.CreateAsync(new Tag(TagType.PreparationTime, name));
            return tag;
        }
    }
}
