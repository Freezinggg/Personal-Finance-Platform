using PersonalFinancePlatform.Domain.Transaction.Entities;

namespace PersonalFinancePlatform.API.Contracts.Transaction
{
    public class UpdateTransactionRequest
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionAt { get; set; }
    }
}
