using PersonalFinancePlatform.Domain.Transaction.Entities;

namespace PersonalFinancePlatform.API.Contracts.Transaction
{
    public class RecordTransactionRequest
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionAt { get; set; }
    }
}
