using PersonalFinancePlatform.Application.Record.Authentication;
using PersonalFinancePlatform.Domain.User.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        JwtToken Generate(User user);
    }
}
