using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface IPreparedRecipeImageService
    {
        Task<Response<List<PreparedRecipeImage>>> GetByUserIdAsync(int userId, int pageNumber, int pageSize);
        Task<Response<List<PreparedRecipeImage>>> GetByRecipeIdAsync(int recipeId, int pageNumber, int pageSize);
        Task<Response<PreparedRecipeImage>> UploadImage(User creator, PreparedRecipeImageDTO preparedRecipeImageDTO, bool saveChanges = true);
        Task<Response<bool>> HasUploadedImageForRecipe(int userId, int recipeId);
    }
}
