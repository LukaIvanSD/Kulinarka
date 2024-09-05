using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<Response<List<T>>> GetAllAsync()
        {
            try
            {
                var result = await _dbSet.ToListAsync();
                return Response<List<T>>.Success(result, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<T>>.Failure("Error fetching data: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return Response<T>.Failure("Entity not found", StatusCode.NotFound);
                }
                return Response<T>.Success(entity, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error fetching data: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<T>> CreateAsync(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return Response<T>.Success(entity, StatusCode.Created);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error creating entity: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<T>> UpdateAsync(int id, T updatedEntity)
        {
            try
            {
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity == null)
                {
                    return Response<T>.Failure("Entity not found", StatusCode.NotFound);
                }

                _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                await _context.SaveChangesAsync();
                return Response<T>.Success(updatedEntity, StatusCode.OK);
            }
            catch (DbUpdateException ex)
            {
                return Response<T>.Failure("Database update error: " + ex.Message, StatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error updating entity: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<T>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return Response<T>.Failure("Entity not found", StatusCode.NotFound);
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return Response<T>.Success(entity, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error deleting entity: " + ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
