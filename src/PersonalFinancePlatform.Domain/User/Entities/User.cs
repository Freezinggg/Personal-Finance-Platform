using PersonalFinancePlatform.Domain.Exception;
using PersonalFinancePlatform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Domain.User.Entities
{
    public sealed class User
    {
        public Guid Id { get; }
        public Email Email { get; private set; }
        public string DisplayName { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; }


        public User(Email email, string displayName, string passwordHash, DateTime createdAt)
        {
            if (email is null)
                throw new InvariantViolationException("[Email] cannot be empty.");

            if (string.IsNullOrWhiteSpace(displayName))
                throw new InvariantViolationException("[Display Name] cannot be empty.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new InvariantViolationException("[Password] cannot be empty.");

            Id = Guid.NewGuid();
            Email = email;
            DisplayName = displayName;
            PasswordHash = passwordHash;
            CreatedAt = createdAt;
        }


        public void ChangeDisplayName(string newDisplayName)
        {
            if (string.IsNullOrWhiteSpace(newDisplayName))
                throw new InvariantViolationException("[Display Name] cannot be empty.");

            DisplayName = newDisplayName.Trim();
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new InvariantViolationException("[Password] cannot be empty.");

            PasswordHash = newPasswordHash;
        }
    }
}
