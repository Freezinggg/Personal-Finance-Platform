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
                throw new InvariantViolationException("[Wallet Name] cannot be empty.");

            if (ownerId == Guid.Empty)
                throw new InvariantViolationException("[Owner ID] cannot be empty.");

            OwnerId = ownerId;
            WalletName = walletName;
            Balance = 0;
            CreatedAt = createdAt;
        }

        public void Rename(string newName)
        {
            if(string.IsNullOrWhiteSpace(newName))
                throw new InvariantViolationException("[Wallet Name] cannot be empty.");

            WalletName = newName;
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
                    DecreaseBalance(amount);
                    break;
            }
        }
    }
}
