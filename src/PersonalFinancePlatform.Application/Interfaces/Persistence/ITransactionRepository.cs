using PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface ITransactionRepository
    {
        void Add(Domain.Transaction.Entities.Transaction transaction);

        //Smell, all filter parameter same, might refactor > create 1 class to populate it.
        Task<int> CountTransactionsAsync(Guid userId, Guid? walletId, TransactionType? type, CancellationToken cancellationToken);
        Task<IReadOnlyList<GetTransactionHistoryResult>> GetTransactionHistoryAsync(
            Guid userId, Guid? walletId, TransactionType? type, int page, int pageSize, CancellationToken cancellationToken);
    }
}
