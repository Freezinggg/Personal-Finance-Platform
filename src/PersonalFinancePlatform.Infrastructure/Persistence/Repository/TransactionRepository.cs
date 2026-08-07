using Microsoft.EntityFrameworkCore;
using PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Repository
{
    public sealed class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
    {
        //private sealed record TransactionHistoryProjection(Transaction Transaction, Wallet Wallet);
        private sealed class TransactionHistoryProjection
        {
            public Transaction Transaction { get; init; } = default!;
            public Wallet Wallet { get; init; } = default!;
        }

        private readonly AppDbContext _dbContext = dbContext;

        public void Add(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
        }

        private IQueryable<TransactionHistoryProjection> BuildBaseTransactionHistoryQuery(Guid userId)
        {
            var query = from transaction in _dbContext.Transactions.AsNoTracking()
                        join wallet in _dbContext.Wallets.AsNoTracking() on transaction.WalletId equals wallet.Id
                        where wallet.OwnerId == userId
                        //select new TransactionHistoryProjection(transaction, wallet);
                        select new TransactionHistoryProjection
                        {
                            Transaction = transaction,
                            Wallet = wallet
                        };

            return query;
        }

        private IQueryable<TransactionHistoryProjection> ApplyFilters(IQueryable<TransactionHistoryProjection> query, GetTransactionHistoryFilter filter)
        {
            if (filter.Type is not null) 
                query = query.Where(x => x.Transaction.TransactionType == filter.Type);

            if (filter.WalletId is not null) 
                query = query.Where(x => x.Wallet.Id == filter.WalletId);

            return query;
        }

        public async Task<int> CountTransactionsAsync(GetTransactionHistoryFilter filter, CancellationToken cancellationToken)
        {
            var query = BuildBaseTransactionHistoryQuery(filter.UserId);
            query = ApplyFilters(query, filter);

            return await query.CountAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<GetTransactionHistoryResult>> GetTransactionHistoryAsync(GetTransactionHistoryFilter filter, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = BuildBaseTransactionHistoryQuery(filter.UserId);
            query = ApplyFilters(query, filter);

            Console.WriteLine(query.ToQueryString());

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
