using MediatR;
using PersonalFinancePlatform.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.DeleteTransaction
{
    public sealed class DeleteTransactionCommand : IRequest<Result<bool>>
    {
        public Guid UserId { get; }
        public Guid TransactionId { get; }

        public DeleteTransactionCommand(Guid userId, Guid transactionId)
        {
            UserId = userId;
            TransactionId = transactionId;
        }
    }
}
