using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Wallet.CreateWallet;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Wallet.DeleteWallet
{
    public class DeleteWalletHandler(
        IWalletRepository walletRepository
        ) : IRequestHandler<DeleteWalletCommand, Result<bool>>
    {
        private readonly IWalletRepository _walletRepo = walletRepository;

        public async Task<Result<bool>> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _walletRepo.GetByIdAndOwnerAsync(request.WalletId, request.UserId, cancellationToken);
                if (wallet is null)
                    return Result<bool>.NotFound("Wallet doesn't exist.");

                await _walletRepo.DeleteAsync(wallet, cancellationToken);

                return Result<bool>.Success(true);
            }
            catch (DomainException ex)
            {
                //This is domain exception, which is to check INVARIANT
                switch (ex.Category)
                {
                    case FailureCategory.Invariant:
                        return Result<bool>.Invalid(ex.Message);
                    case FailureCategory.Policy or FailureCategory.State:
                        return Result<bool>.Fail(ex.Message);
                    default:
                        return Result<bool>.Error("Unhandled domain exception.");
                }
            }
            catch
            {
                return Result<bool>.Error("An unexpected error occurred..");
            }
        }
    }
}
