using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Auth.Login
{
    public sealed record LoginResult(
        Guid UserId,
        string DisplayName);
}
