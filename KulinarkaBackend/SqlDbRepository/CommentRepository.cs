﻿using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DbSet<Comment> dbSet;
        private readonly IRepository<Comment> repository;
        public CommentRepository(AppDbContext context, IRepository<Comment> repository)
        {
            dbSet = context.Comments;
            this.repository = repository;
        }
        public async Task<Response<Comment>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<Comment>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<Comment>> CreateAsync(Comment entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<Comment>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<Comment>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<Comment>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<List<Comment>>> GetByRecipeIdPagedAsync(int recipeId, int startIndex, int resultSize)
        {
            try { 
                List<Comment> comments = await dbSet
                    .Where(c => c.RecipeId == recipeId)
                    .OrderByDescending(c=>c.DateCreated)
                    .Skip(startIndex)
                    .Take(resultSize)
                    .ToListAsync();
                return Response<List<Comment>>.Success(comments,StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<Comment>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<List<Comment>>> GetByRecipeIdPagedWithCreatorEagerAsync(int recipeId, int startIndex, int resultSize)
        {
            try
            {
                List<Comment> comments = await dbSet
                    .Include(c => c.Creator)
                    .ThenInclude(u => u.UserTitle)
                    .ThenInclude(ut => ut.CurrentTitle)
                    .Where(c => c.RecipeId == recipeId)
                    .OrderByDescending(c => c.DateCreated)
                    .Skip(startIndex)
                    .Take(resultSize)
                    .ToListAsync();
                return Response<List<Comment>>.Success(comments, StatusCode.OK);
            }
            catch (Exception ex)
            {
                return Response<List<Comment>>.Failure(ex.Message, StatusCode.InternalServerError);
            }
        }

        public async Task<Response<Comment>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<Comment>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<Comment>> UpdateAsync(int id, Comment entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
