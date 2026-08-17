using PersonalFinancePlatform.Domain.Category.Entities;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface ICategoryRepository
    {
        void Add(Category category);
        Task AddAsync(Category category, CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Category?> GetByIdAndOwnerAsync(Guid id, Guid ownerId, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(Guid ownerId, string categoryName, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(Guid id, Guid ownerId, string categoryName, CancellationToken cancellationToken);
        Task DeleteAsync(Category category, CancellationToken cancellationToken);
    }
}
