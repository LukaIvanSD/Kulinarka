using Kulinarka.Models.Responses;

namespace Kulinarka.RepositoryInterfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Response<List<T>>> GetAllAsync();
        Task<Response<T>> GetByIdAsync(int id);
        Task<Response<T>> CreateAsync(T entity);
        Task<Response<T>> UpdateAsync(int id, T entity);
        Task<Response<T>> DeleteAsync(int id);



    }
}
