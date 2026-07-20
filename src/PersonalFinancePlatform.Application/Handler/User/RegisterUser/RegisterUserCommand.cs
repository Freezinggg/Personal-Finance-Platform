using MediatR;
using PersonalFinancePlatform.Application.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.User.RegisterUser
{
    public sealed class RegisterUserCommand : IRequest<Result<RegisterUserResult>>
    {
        public string DisplayName { get;}
        public string Email { get;}
        public string Password { get;}

        public RegisterUserCommand(string displayName, string email, string password)
        {
            DisplayName = displayName;
            Email = email;
            Password = password;
        }
    }
}
