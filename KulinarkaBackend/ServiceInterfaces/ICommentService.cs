using Kulinarka.DTO;
using Kulinarka.Models;
using Kulinarka.Models.Responses;

namespace Kulinarka.ServiceInterfaces
{
    public interface ICommentService
    {
        Task<Response<List<CommentDTO>>> GetByRecipeIdPagedAsync(int recipeId, int pageNumber, int pageSize);
        Task<Response<List<Comment>>> GetByRecipePagedAsync(Recipe recipe, int pageNumber, int pageSize);
        Task<Response<Comment>> CreateAsync(User creator, CreateCommentDTO createCommentDTO, bool saveChanges = true);
        Task<Response<Comment>> RollbackTransactionAsync();
        Task<Response<Comment>> BeginTransactionAsync();
        Task<Response<Comment>> SaveChangesAsync();
        Task<Response<Comment>> CommitTransactionAsync();
        Task<Response<List<Comment>>> GetByRecipePagedWithCreatorEagerAsync(Recipe recipe, int pageNumber, int pageSize);
    }
}
