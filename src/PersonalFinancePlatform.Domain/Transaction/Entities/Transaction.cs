using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.Transaction.Entities
{ 
    public enum TransactionType : int
    {
        Income = 1,
        Expense = 2
    }

    public sealed class Transaction
    {
        public Guid Id { get; }
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public string Description { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public DateTime TransactionAt { get; private set; } //Time which the transaction happens
        public DateTime CreatedAt { get; } //Time which it created in the system


        public Transaction(Guid walletId, decimal amount, string description, TransactionType transactionType, DateTime transactionAt, DateTime createdAt)
        {
            if (walletId == Guid.Empty)
                throw new InvariantViolationException("Please pick a wallet.");

            if (amount <= 0)
                throw new InvariantViolationException("Transaction amount must be > 0.");

            if (string.IsNullOrWhiteSpace(description))
                throw new InvariantViolationException("Transaction description cannot be empty.");

            Id = Guid.NewGuid();
            WalletId = walletId;
            Amount = amount;
            Description = description;
            TransactionType = transactionType;
            TransactionAt = transactionAt;
            CreatedAt = createdAt;
        }

        //Smell, parameters too much. consider making object (refactor later)
        public void EditTransaction(Guid walletId, decimal amount, string description, TransactionType transactionType, DateTime transactionAt)
        {
            /*Add walletId as parameter because the user might input the wrong wallet, so changes between wallet
             is okay, not transfer
             */
            if (walletId == Guid.Empty)
                throw new InvariantViolationException("Please pick a wallet.");

            if (amount <= 0)
                throw new InvariantViolationException("Transaction amount must be > 0.");

            if (string.IsNullOrWhiteSpace(description))
                throw new InvariantViolationException("Transaction description cannot be empty.");

            WalletId = walletId;
            Amount = amount;
            Description = description;
            TransactionType = transactionType;
            TransactionAt = transactionAt;
        }
    }

    
}
