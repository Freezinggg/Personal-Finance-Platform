using Microsoft.EntityFrameworkCore;
using PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Repository
{
    public sealed class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public void Add(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
        }

        public async Task<int> CountTransactionsAsync(
            Guid userId,
            Guid? walletId,
            TransactionType? type,
            CancellationToken cancellationToken)
        {
            //Smell, implementation same as GetTransactionHistoryAsync(), would refactor later.
            var query = from transaction in _dbContext.Transactions.AsNoTracking()
                        join wallet in _dbContext.Wallets.AsNoTracking() on transaction.WalletId equals wallet.Id
                        select new
                        {
                            Transaction = transaction,
                            Wallet = wallet
                        };

            query = query.Where(x => x.Wallet.OwnerId == userId);
            if (type is not null)
                query = query.Where(x => x.Transaction.TransactionType == type);

            if (walletId is not null)
                query = query.Where(x => x.Wallet.Id == walletId);

            return await query.CountAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<GetTransactionHistoryResult>> GetTransactionHistoryAsync(
            Guid userId, 
            Guid? walletId, 
            TransactionType? type,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            //Join table transaction and wallet
            var query = from transaction in _dbContext.Transactions.AsNoTracking()
                    join wallet in _dbContext.Wallets.AsNoTracking() on transaction.WalletId equals wallet.Id
                    select new
                    {
                        Transaction = transaction,
                        Wallet = wallet
                    };

            //Filter based on user
            query = query.Where(x => x.Wallet.OwnerId == userId);

            //Filter type
            if(type is not null)
                query = query.Where(x => x.Transaction.TransactionType == type);

            if (walletId is not null)
                query = query.Where(x => x.Wallet.Id == walletId);

            var items = await query
                .OrderByDescending(x => x.Transaction.TransactionAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new GetTransactionHistoryResult(
                   x.Transaction.Id,
                   x.Wallet.WalletName,
                   x.Transaction.TransactionType.ToString(),
                   x.Transaction.Amount,
                   x.Transaction.Description,
                   x.Transaction.TransactionAt
                    ))
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
