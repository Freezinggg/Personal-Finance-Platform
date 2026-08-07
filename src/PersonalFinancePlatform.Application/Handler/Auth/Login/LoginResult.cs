using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Auth.Login
{
    public sealed record LoginResult(
        string accessToken,
        DateTime expiresAt,
        Guid UserId,
        string DisplayName);
}
