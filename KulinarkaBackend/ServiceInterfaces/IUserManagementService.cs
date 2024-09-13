using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IUserManagementService
    {
        Task<Response<User>> AddProgress(int userId, RequirementType requirementType);
        Task<Response<User>> RemoveProgress(int userId, RequirementType requirementType);
    }
}
