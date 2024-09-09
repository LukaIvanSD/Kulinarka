using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace Kulinarka.SqlDbRepository
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        private IDbContextTransaction _transaction;

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

        public async Task<Response<T>> CreateAsync(T entity,bool saveChanges=true)
        {
            try
            {
                _dbSet.Add(entity);
                if (saveChanges)
                    await _context.SaveChangesAsync();
                return Response<T>.Success(entity, StatusCode.Created);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error creating entity: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<T>> UpdateAsync(int id, T updatedEntity,bool saveChanges=true)
        {
            try
            {
                var existingEntity = await _dbSet.FindAsync(id);
                if (existingEntity == null)
                {
                    return Response<T>.Failure("Entity not found", StatusCode.NotFound);
                }
                _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                if (saveChanges)
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

        public async Task<Response<T>> DeleteAsync(int id,bool saveChanges=true)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return Response<T>.Failure("Entity not found", StatusCode.NotFound);
                }

                _dbSet.Remove(entity);
                if (saveChanges)
                    await _context.SaveChangesAsync();
                return Response<T>.Success(entity, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error deleting entity: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<T>> BeginTransactionAsync()
        {
            try
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                return Response<T>.Success(null, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error starting transaction: " + ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<T>> CommitTransactionAsync()
        {
            if (_transaction == null)
                return Response<T>.Failure("No active transaction to commit.", StatusCode.BadRequest);
            try
            {
                await _transaction.CommitAsync();
                return Response<T>.Success(null, StatusCode.OK);

            }
            catch (OperationCanceledException ex)
            {
                return Response<T>.Failure("Transaction commit cancelled: " + ex.Message, StatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error committing transaction: " + ex.Message, StatusCode.InternalServerError);
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task<Response<T>> RollbackTransactionAsync()
        {
            if (_transaction == null)
                return Response<T>.Failure("No active transaction to rollback.", StatusCode.BadRequest);
            try
            {
                await _transaction.RollbackAsync();
                return Response<T>.Success(null, StatusCode.OK);
            }
            catch (OperationCanceledException ex)
            {
                return Response<T>.Failure("Transaction commit cancelled: " + ex.Message, StatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error rolling back transaction: " + ex.Message, StatusCode.InternalServerError);
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
        public async Task<Response<T>> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return Response<T>.Success(null, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<T>.Failure("Error saving changes: " + ex.Message, StatusCode.InternalServerError);
            }
        }
    }
}
