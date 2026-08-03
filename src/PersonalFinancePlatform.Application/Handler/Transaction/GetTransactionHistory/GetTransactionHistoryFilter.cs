using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory
{
    public record GetTransactionHistoryFilter(Guid UserId, Guid? WalletId, TransactionType? Type);
}
