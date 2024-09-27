﻿using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Kulinarka.SqlDbRepository
{
    public class PreparedRecipeImageRepository : IPreparedRecipeImageRepository
    {
        private readonly DbSet<PreparedRecipeImage> dbSet;
        private readonly IRepository<PreparedRecipeImage> repository;
        public PreparedRecipeImageRepository(AppDbContext context, IRepository<PreparedRecipeImage> repository)
        {
            dbSet = context.PreparedRecipeImages;
            this.repository = repository;
        }
        public async Task<Response<PreparedRecipeImage>> BeginTransactionAsync()
        {
            return await repository.BeginTransactionAsync();
        }

        public async Task<Response<PreparedRecipeImage>> CommitTransactionAsync()
        {
            return await repository.CommitTransactionAsync();
        }

        public async Task<Response<PreparedRecipeImage>> CreateAsync(PreparedRecipeImage entity, bool saveChanges = true)
        {
            return await repository.CreateAsync(entity, saveChanges);
        }

        public async Task<Response<PreparedRecipeImage>> DeleteAsync(int id, bool saveChanges = true)
        {
            return await repository.DeleteAsync(id, saveChanges);
        }

        public async Task<Response<List<PreparedRecipeImage>>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Response<PreparedRecipeImage>> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Response<PreparedRecipeImage>> RollbackTransactionAsync()
        {
            return await repository.RollbackTransactionAsync();
        }

        public async Task<Response<PreparedRecipeImage>> SaveChangesAsync()
        {
            return await repository.SaveChangesAsync();
        }

        public async Task<Response<PreparedRecipeImage>> UpdateAsync(int id, PreparedRecipeImage entity, bool saveChanges = true)
        {
            return await repository.UpdateAsync(id, entity, saveChanges);
        }
    }
}
