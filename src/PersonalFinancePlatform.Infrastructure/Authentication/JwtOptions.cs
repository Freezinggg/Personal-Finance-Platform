using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Authentication
{
    public sealed class JwtOptions
    {
        public string Secret { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public int ExpirationMinutes { get; init; }
    }
}
