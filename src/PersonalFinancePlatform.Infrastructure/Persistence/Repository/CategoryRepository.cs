using Microsoft.EntityFrameworkCore;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Category.Entities;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Repository
{
    public sealed class CategoryRepository(AppDbContext dbContext) : ICategoryRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public void Add(Category category)
        {
            _dbContext.Categories.Add(category);
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> ExistsByNameAsync(Guid ownerId, string categoryName, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .AsNoTracking()
                .AnyAsync(
                    x => x.OwnerId == ownerId &&
                         x.CategoryName == categoryName,
                    cancellationToken);
        }

        public Task<bool> ExistsByNameAsync(Guid id, Guid ownerId, string categoryName, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .AsNoTracking()
                .AnyAsync(
                    x => x.OwnerId == ownerId &&
                         x.CategoryName == categoryName &&
                         x.Id != id,
                    cancellationToken);
        }

        public Task<Category?> GetByIdAndOwnerAsync(Guid id, Guid ownerId, CancellationToken cancellationToken)
        {
            return _dbContext.Categories
                .FirstOrDefaultAsync(
                    x => x.OwnerId == ownerId &&
                        x.Id == id,
                    cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Categories.FindAsync(id, cancellationToken);
        }
    }
}
