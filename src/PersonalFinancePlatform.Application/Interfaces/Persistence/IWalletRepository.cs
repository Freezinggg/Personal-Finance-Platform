using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface IWalletRepository
    {
        void Add(Wallet wallet);
        Task AddAsync(Wallet wallet, CancellationToken cancellationToken);
        Task<Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(Guid ownerId, string walletName,  CancellationToken cancellationToken);
    }
}
