using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Response<List<T>>> GetAllAsync();
        Task<Response<T>> GetByIdAsync(int id);
        Task<Response<T>> CreateAsync(T entity,bool saveChanges=true);
        Task<Response<T>> UpdateAsync(int id, T entity, bool saveChanges = true);
        Task<Response<T>> DeleteAsync(int id, bool saveChanges = true);
        Task<Response<T>> BeginTransactionAsync();
        Task<Response<T>> CommitTransactionAsync();
        Task<Response<T>> RollbackTransactionAsync();
        Task<Response<T>> SaveChangesAsync();



    }
}
