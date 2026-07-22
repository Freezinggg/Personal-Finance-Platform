using PersonalFinancePlatform.Application.Interfaces.Security;
using PersonalFinancePlatform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Security.PasswordHasher
{
    public sealed class BCryptPasswordHasher : IPasswordHasher
    {
        public PasswordHash Hash(Password password)
        {
            PasswordHash hashed = new PasswordHash(BCrypt.Net.BCrypt.HashPassword(password.Value));
            return hashed;
        }

        public bool Verify(Password password, PasswordHash passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password.Value, passwordHash.Value);
        }
    }
}
