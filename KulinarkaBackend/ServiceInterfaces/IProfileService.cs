using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IProfileService
    {
        public Task<Response<ProfileDTO>> GetProfileDTO(int userId);
        public Task<Response<User>> UpdatePasswordAsync(User user, PasswordChangeRequst passwordChangeRequst);
        public Task<Response<string>> UpdatePictureAsync(User user, byte[] pictureBytes);
        public Task<Response<User>> UpdateUserInfoAsync(User user, UserInfoDTO newUserInfo);
    }
}
