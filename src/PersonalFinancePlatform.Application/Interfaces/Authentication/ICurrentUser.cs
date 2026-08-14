using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Authentication
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
    }
}
