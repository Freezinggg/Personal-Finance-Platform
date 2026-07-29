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

        public async Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Wallets.FindAsync(id, cancellationToken);
        }
    }
}
