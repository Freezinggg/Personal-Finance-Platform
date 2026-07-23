using PersonalFinancePlatform.Domain.Exception;
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

        public void IncreaseBalance(decimal amount)
        {
            if(amount <= 0)
                throw new InvariantViolationException("[Amount] cannot be 0 or negative.");

            Balance += amount;
        }

        public void DecreaseBalance(decimal amount)
        {
            if (amount <= 0)
                throw new InvariantViolationException("[Amount] cannot be 0 or negative.");

            Balance -= amount;
        }
    }
}
