using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Application.Interfaces.Security;
using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.User.RegisterUser
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

            try
            {
                // Check email uniqueness
                var existingUser = await _userRepo.FindByEmailAsync(request.Email, cancellationToken);
                if (existingUser is not null)
                    return Result<RegisterUserResult>.Invalid("Email already taken, please use another email.");

                // Hash password
                string hashedPassword = await _passwordHasher.GeneratePasswordAsync(request.Password,cancellationToken);

                // Create User
                await _uow.BeginAsync(cancellationToken);
                Guid newUserId = Guid.NewGuid();

                Domain.User.Entities.User user =
                    new Domain.User.Entities.User(
                        new Email(request.Email), 
                        request.DisplayName, 
                        hashedPassword,
                        now);

                // Create default Wallet. Not yet implemented, create another issue/ticket

                // Save User
                await _userRepo.AddAsync(user, cancellationToken);

                // Save Wallet. Not yet implemented, create another issue/ticket

                // Commit
                await _uow.CommitAsync(cancellationToken);

                // Return success
                return Result<RegisterUserResult>.Success(new RegisterUserResult(user.Id));
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync(cancellationToken);
                return Result<RegisterUserResult>.ServiceUnavailable("Register User service unavailable.");
            }
        }
    }
}
