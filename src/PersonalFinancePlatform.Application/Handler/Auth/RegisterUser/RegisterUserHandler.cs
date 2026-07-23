using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Application.Interfaces.Security;
using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.User.ValueObjects;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Auth.RegisterUser
{
    public class RegisterUserHandler(IUserRepository userRepository, IWalletRepository walletRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>>
    {

        private readonly IUserRepository _userRepo = userRepository;
        private readonly IWalletRepository _walletRepo = walletRepository;

        private readonly IUnitOfWork _uow = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;

            Email email = new Email(request.Email);
            // Check email uniqueness
            var existingUser = await _userRepo.FindByEmailAsync(email, cancellationToken);
            if (existingUser is not null)
                return Result<RegisterUserResult>.Invalid("Email already taken, please use another email.");

            // Hash password
            Password password = new Password(request.Password);
            PasswordHash hashedPassword = _passwordHasher.Hash(password);

            // Create User
            User user = new User(email, request.DisplayName, hashedPassword, now);

            await _uow.BeginAsync(cancellationToken);
            try
            {
                // Save User
                _userRepo.Add(user);

                // Create and save wallet
                Wallet wallet = new Wallet(user.Id, "Cash", now);
                _walletRepo.Add(wallet);

                // Commit
                await _uow.CommitAsync(cancellationToken);
            }
            catch
            {
                await _uow.RollbackAsync(cancellationToken);
                return Result<RegisterUserResult>.Error("Unhandled domain exception.");
            }

            // Return success
            return Result<RegisterUserResult>.Success(new RegisterUserResult(user.Id));
        }
    }
}
