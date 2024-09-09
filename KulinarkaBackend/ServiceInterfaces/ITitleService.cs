using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface ITitleService
    {
        Task<Response<Title>> GetNextTitle(int currentTitleId);
    }
}
