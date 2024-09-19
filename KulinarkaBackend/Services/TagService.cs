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

        public Task<Response<List<Tag>>> GetAllAsync()
        {
            return tagRepository.GetAllAsync();
        }

        public async Task<Response<Tag>> GetByIdAsync(int id)
        {
            return await tagRepository.GetByIdAsync(id);
        }
    }
}
