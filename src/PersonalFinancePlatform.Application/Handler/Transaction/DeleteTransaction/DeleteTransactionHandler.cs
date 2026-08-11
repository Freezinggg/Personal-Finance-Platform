using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Transaction.UpdateTransaction;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Exception;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Transaction.DeleteTransaction
{
    public class DeleteTransactionHandler(
        ITransactionRepository transactionRepository,
             IWalletRepository walletRepository,
             IUnitOfWork unitOfWork
        ) : IRequestHandler<DeleteTransactionCommand, Result<bool>>
    {
        private readonly ITransactionRepository _transactionRepo = transactionRepository;
        private readonly IWalletRepository _walletRepo = walletRepository;
        private readonly IUnitOfWork _uow = unitOfWork;

        public async Task<Result<bool>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
        {
            bool transactionStarted = false;

            try
            {
                var transaction = await _transactionRepo.FindByIdForUserAsync(request.TransactionId, request.UserId, cancellationToken);
                if (transaction is null)
                    return Result<bool>.NotFound("Transaction doesn't exist.");

                var wallet = await _walletRepo.GetByIdAsync(transaction.WalletId, cancellationToken);
                if (wallet is null)
                    return Result<bool>.Error("Invalid wallet. Please make sure you choose the right wallet.");

                await _uow.BeginAsync(cancellationToken);

                //Revert back old transaction from wallet, make sure non-negative
                wallet.RevertTransaction(transaction);
                wallet.EnsureValidBalance();

                //Delete/remove transaction
                _transactionRepo.Delete(transaction);

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
                if (transactionStarted) await _uow.RollbackAsync(cancellationToken);

                return Result<bool>.Error("An unexpected error occurred..");
            }
        }
    }
}
