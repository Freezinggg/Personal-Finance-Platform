using PersonalFinancePlatform.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Security.PasswordHasher
{
    public sealed class BCryptPasswordHasher : IPasswordHasher
    {
        public string GeneratePassword(string password, CancellationToken cancellationToken)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash, CancellationToken cancellationToken)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
