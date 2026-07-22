using PersonalFinancePlatform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        PasswordHash Hash(Password password);

        bool Verify(Password password, PasswordHash passwordHash);
    }
}
