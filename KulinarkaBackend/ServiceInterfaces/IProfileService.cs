using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IProfileService
    {
        public Task<Response<ProfileDTO>> GetProfileDTO(int userId);
    }
}
