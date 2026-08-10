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
        Task<Domain.Transaction.Entities.Transaction?> FindByIdForUserAsync(Guid transactionId, Guid userId, CancellationToken cancellationToken);

        //Smell, all filter parameter same, might refactor > create 1 class to populate it.
        Task<int> CountTransactionsAsync(GetTransactionHistoryFilter filter, CancellationToken cancellationToken);
        Task<IReadOnlyList<GetTransactionHistoryResult>> GetTransactionHistoryAsync(
            GetTransactionHistoryFilter filter, int page, int pageSize, CancellationToken cancellationToken);
    }
}
