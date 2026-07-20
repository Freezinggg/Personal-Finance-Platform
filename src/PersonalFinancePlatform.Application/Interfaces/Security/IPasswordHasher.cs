using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Security
{
    public interface IPasswordHasher
    {
        Task<string> GeneratePasswordAsync(string password, CancellationToken cancellationToken);
    }
}
