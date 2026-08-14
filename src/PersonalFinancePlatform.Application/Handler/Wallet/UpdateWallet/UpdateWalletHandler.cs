using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Exception;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Wallet.UpdateWallet
{
    public class UpdateWalletHandler(
        IWalletRepository walletRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<UpdateWalletCommand, Result<bool>>
    {
        private readonly IWalletRepository _walletRepo = walletRepository;
        private readonly IUnitOfWork _uow = unitOfWork;

        public async Task<Result<bool>> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var wallet = await _walletRepo.GetByIdAndOwnerAsync(request.WalletId, request.UserId, cancellationToken);
                if (wallet is null)
                    return Result<bool>.NotFound("Wallet doesn't exist.");

                var existsWallet = await _walletRepo.ExistsByNameAsync(request.WalletId, request.UserId, request.WalletName, cancellationToken);
                if (existsWallet)
                    return Result<bool>.Fail("Wallet with same name already exist");

                await _uow.BeginAsync(cancellationToken);
                wallet.Rename(request.WalletName);
                await _uow.CommitAsync(cancellationToken);

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
