using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.User
{
    public sealed class User
    {
        public Guid Id { get; }
        public string Email { get; private set; } 
        public string DisplayName { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; }


        public User(Guid id, string email, string displayName, string passwordHash, DateTime createdAt)
        {
            if(id == Guid.Empty)
                throw new InvariantViolationException("[User ID] cannot be empty.");

            if (string.IsNullOrWhiteSpace(email))
                throw new InvariantViolationException("[Email] cannot be empty.");

            if (string.IsNullOrWhiteSpace(displayName))
                throw new InvariantViolationException("[Display Name] cannot be empty.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new InvariantViolationException("[Password] cannot be empty.");

            Id = id;
            Email = email;
            DisplayName = displayName;
            PasswordHash = passwordHash;
            CreatedAt = createdAt;
        }


        public void ChangeDisplayName(string newDisplayName)
        {
            if (string.IsNullOrWhiteSpace(newDisplayName))
                throw new InvariantViolationException("[Display Name] cannot be empty.");

            DisplayName = newDisplayName;
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}
