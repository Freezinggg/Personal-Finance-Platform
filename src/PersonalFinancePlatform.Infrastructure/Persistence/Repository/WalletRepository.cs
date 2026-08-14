using Microsoft.EntityFrameworkCore;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Repository
{
    public sealed class WalletRepository(AppDbContext dbContext) : IWalletRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public void Add(Wallet wallet)
        {
            _dbContext.Wallets.Add(wallet);
        }

        public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            await _dbContext.Wallets.AddAsync(wallet, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Wallet wallet, CancellationToken cancellationToken)
        {
            _dbContext.Wallets.Remove(wallet);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> ExistsByNameAsync(Guid ownerId, string walletName, CancellationToken cancellationToken)
        {
            return _dbContext.Wallets
                .AsNoTracking()
                .AnyAsync(
                    x => x.OwnerId == ownerId &&
                         x.WalletName == walletName,
                    cancellationToken);
        }

        public Task<bool> ExistsByNameAsync(Guid id, Guid ownerId, string walletName, CancellationToken cancellationToken)
        {
            return _dbContext.Wallets
            .AsNoTracking()
            .AnyAsync(
                x => x.OwnerId == ownerId &&
                     x.WalletName == walletName &&
                     x.Id != id,
                cancellationToken);
        }

        public Task<Wallet?> GetByIdAndOwnerAsync(Guid id, Guid ownerId, CancellationToken cancellationToken)
        {
            return _dbContext.Wallets
                .FirstOrDefaultAsync(
                    x => x.OwnerId == ownerId &&
                        x.Id == id,
                    cancellationToken);
        }

        public async Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Wallets.FindAsync(id, cancellationToken);
        }
    }
}
