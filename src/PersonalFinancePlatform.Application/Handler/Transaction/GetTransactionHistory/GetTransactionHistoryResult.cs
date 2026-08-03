using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory
{
    public record class GetTransactionHistoryResult(
        Guid Id,
        string WalletName,
        string TransactionType,
        decimal Amount,
        string Description,
        DateTime TransactionDate
        );
}
