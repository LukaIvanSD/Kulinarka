
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface ITagService
    {
        Task<Response<List<Tag>>> GetAllAsync();
        Task<Response<Tag>> GetByIdAsync(int id);
    }
}
