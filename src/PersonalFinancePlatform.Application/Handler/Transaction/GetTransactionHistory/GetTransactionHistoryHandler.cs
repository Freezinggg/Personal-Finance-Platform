using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.GetTransactionHistory
{
    public sealed class GetTransactionHistoryHandler(
        ITransactionRepository transactionRepository
        ) : IRequestHandler<GetTransactionHistoryQuery, Result<PagedResult<GetTransactionHistoryResult>>>
    {
        private readonly ITransactionRepository _transactionRepo = transactionRepository;

        public async Task<Result<PagedResult<GetTransactionHistoryResult>>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var filter = new GetTransactionHistoryFilter(request.UserId, request.WalletId, request.TransactionType);

            var totalItems = await _transactionRepo.CountTransactionsAsync(filter, cancellationToken);
            var items = await _transactionRepo.GetTransactionHistoryAsync(filter, request.Page, request.PageSize, cancellationToken);

            PagedResult<GetTransactionHistoryResult> pagedResult = new(
                items,
                request.Page,
                request.PageSize,
                totalItems);

            return Result<PagedResult<GetTransactionHistoryResult>>.Success(pagedResult);
        }
    }
}
