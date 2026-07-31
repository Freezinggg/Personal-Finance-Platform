using PersonalFinancePlatform.Domain.Transaction.Entities;

namespace PersonalFinancePlatform.API.Contracts.Transaction
{
    public record class GetTransactionHistoryRequest(
        Guid? WalletId,
        TransactionType? TransactionType,
        int Page = 1,
        int PageSize = 20
        );
}
