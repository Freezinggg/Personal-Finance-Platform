using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;
using PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Exception;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Wallet.CreateWallet
{
    public class CreateWalletHandler(
        IWalletRepository walletRepository) : IRequestHandler<CreateWalletCommand, Result<Guid>>
    {
        private readonly IWalletRepository _walletRepo = walletRepository;

        public async Task<Result<Guid>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = new Domain.Wallet.Entities.Wallet(request.UserId, request.WalletName, DateTime.UtcNow);
                var existsWallet = await _walletRepo.ExistsByNameAsync(request.UserId, wallet.WalletName, cancellationToken);
                if (existsWallet)
                    return Result<Guid>.Invalid("Wallet with same name already exist");

                await _walletRepo.AddAsync(wallet, cancellationToken);
                return Result<Guid>.Success(wallet.Id);
            }
            catch (DomainException ex)
            {
                //This is domain exception, which is to check INVARIANT
                switch (ex.Category)
                {
                    case FailureCategory.Invariant:
                        return Result<Guid>.Invalid(ex.Message);
                    case FailureCategory.Policy or FailureCategory.State:
                        return Result<Guid>.Fail(ex.Message);
                    default:
                        return Result<Guid>.Error("Unhandled domain exception.");
                }
            }
            catch
            {
                return Result<Guid>.Error("An unexpected error occurred..");
            }
        }
    }
}
