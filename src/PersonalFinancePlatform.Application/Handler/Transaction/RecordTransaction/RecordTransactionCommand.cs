using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction
{
    public sealed class RecordTransactionCommand : IRequest<Result<RecordTransactionResult>>
    {
        public Guid WalletId { get;  }
        public decimal Amount { get;  }
        public string Description { get; }
        public TransactionType TransactionType { get;  }
        public DateTime TransactionAt { get; }

        public RecordTransactionCommand(Guid walletId, decimal amount, string description, TransactionType transactionType, DateTime transactionAt)
        {
            WalletId = walletId;
            Amount = amount;
            Description = description;
            TransactionType = transactionType;
            TransactionAt = transactionAt;
        }
    }
}
