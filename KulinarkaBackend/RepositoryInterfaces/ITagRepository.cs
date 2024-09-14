using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<Response<Tag>> GetByTypeAndNameAsync(TagType preparationTime, string name);
    }
}
