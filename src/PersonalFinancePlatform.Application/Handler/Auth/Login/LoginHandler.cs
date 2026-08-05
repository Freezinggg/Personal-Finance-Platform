using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Application.Interfaces.Security;
using PersonalFinancePlatform.Domain.Exception;
using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Auth.Login
{
    public sealed class LoginHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher
        ) : IRequestHandler<LoginCommand, Result<LoginResult>>
    {
        private readonly IUserRepository _userRepo = userRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Validate email
                var email = new Email(request.Email);

                //Check if user with email exist
                User? user = await _userRepo.FindByEmailAsync(email, cancellationToken);
                if (user is null)
                    return Result<LoginResult>.Fail("Invalid email or password.");

                //Verify password
                if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
                    return Result<LoginResult>.Fail("Invalid email or password.");

                return Result<LoginResult>.Success(new LoginResult(user.Id, user.DisplayName));
            }
            catch (DomainException ex)
            {
                //This is domain exception, which is to check INVARIANT

                switch (ex.Category)
                {
                    case FailureCategory.Invariant:
                        return Result<LoginResult>.Invalid(ex.Message);
                    case FailureCategory.Policy or FailureCategory.State:
                        return Result<LoginResult>.Fail(ex.Message);
                    default:
                        return Result<LoginResult>.Error("Unhandled domain exception.");
                }
            }
            catch
            {
                return Result<LoginResult>.Error("Unhandled domain exception.");
            }
            
        }
    }
}
