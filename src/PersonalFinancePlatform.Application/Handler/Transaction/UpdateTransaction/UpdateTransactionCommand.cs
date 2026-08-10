using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.UpdateTransaction
{
    public sealed class UpdateTransactionCommand : IRequest<Result<UpdateTransactionResult>>
    {
        public Guid UserId { get; }
        public Guid TransactionId { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public TransactionType TransactionType { get; }
        public DateTime TransactionAt { get; }

        public UpdateTransactionCommand(Guid userId, Guid transactionId, decimal amount, string description, TransactionType transactionType, DateTime transactionAt)
        {
            UserId = userId;
            TransactionId = transactionId;
            Amount = amount;
            Description = description;
            TransactionType = transactionType;
            TransactionAt = transactionAt;
        }
    }
}
