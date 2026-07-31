using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory
{
    public sealed class GetTransactionHistoryQuery : IRequest<Result<PagedResult<GetTransactionHistoryResult>>>
    {
        public Guid UserId { get; }
        public Guid? WalletId { get; }
        public TransactionType? TransactionType { get; }
        public int Page { get; }
        public int PageSize { get; }

        public GetTransactionHistoryQuery(Guid userId, Guid? walletId, TransactionType? transactionType, int page, int pageSize)
        {
            UserId = userId;
            WalletId = walletId;
            TransactionType = transactionType;
            Page = page;
            PageSize = pageSize;
        }
    }
}
