using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface ICommentService
    {
        Task<Response<List<Comment>>> GetByRecipeIdPagedAsync(int recipeId, int pageNumber, int pageSize);
        Task<Response<List<Comment>>> GetByRecipePagedAsync(Recipe recipe, int pageNumber, int pageSize);
        Task<Response<Comment>> CreateAsync(User creator, CreateCommentDTO createCommentDTO);
    }
}
