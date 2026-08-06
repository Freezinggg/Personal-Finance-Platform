using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Record.Authentication
{
    public sealed record JwtToken(string AccessToken, DateTime ExpiresAt);
}
