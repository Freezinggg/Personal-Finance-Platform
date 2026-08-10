using PersonalFinancePlatform.Domain.Exception;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.Wallet.Entities
{
    public sealed class Wallet
    {
        public Guid Id { get; }
        public Guid OwnerId { get; } //UserId
        public string WalletName { get; private set; }
        public decimal Balance { get; private set; }
        public DateTime CreatedAt { get; }


        public Wallet(Guid ownerId, string walletName,  DateTime createdAt)
        {
            Id = Guid.NewGuid();

            if(string.IsNullOrWhiteSpace(walletName))
                throw new InvariantViolationException("Name cannot be empty.");

            if (ownerId == Guid.Empty)
                throw new InvariantViolationException("User cannot be empty.");

            OwnerId = ownerId;
            WalletName = walletName;
            Balance = 0;
            CreatedAt = createdAt;
        }

        public void Rename(string newName)
        {
            if(string.IsNullOrWhiteSpace(newName))
                throw new InvariantViolationException("Name cannot be empty.");

            WalletName = newName;
        }

        public void EnsureSufficientBalance(decimal amount)
        {
            if((Balance - amount) < 0)
                throw new InvariantViolationException("Insufficient funds.");
        }

        public void EnsureValidBalance()
        {
            if(Balance < 0)
                throw new InvariantViolationException("Balance should be valid.");
        }

        //Can only be accessed within this domain.
        private void IncreaseBalance(decimal amount) => Balance += amount;
        private void DecreaseBalance(decimal amount) => Balance -= amount;

        //Function name is Apply because we are trying to Apply financial event/record.
        public void ApplyTransaction(Domain.Transaction.Entities.Transaction transaction)
        {
            decimal amount = transaction.Amount;
            switch (transaction.TransactionType)
            {
                case TransactionType.Income:
                    IncreaseBalance(amount);
                    break;
                case TransactionType.Expense:
                    //Make sure the balance wouldnt go negative if deducted by amount
                    EnsureSufficientBalance(amount);
                    DecreaseBalance(amount);
                    break;
            }
        }

        public void RevertTransaction(Domain.Transaction.Entities.Transaction transaction)
        {
            decimal amount = transaction.Amount;
            switch (transaction.TransactionType)
            {
                case TransactionType.Income:
                    DecreaseBalance(amount);
                    break;
                case TransactionType.Expense:
                    IncreaseBalance(amount);
                    break;
            }
        }
    }
}
