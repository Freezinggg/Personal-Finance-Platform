using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        string GeneratePassword(string password, CancellationToken cancellationToken);

        bool VerifyPassword(string password, string passwordHash, CancellationToken cancellationToken);
    }
}
