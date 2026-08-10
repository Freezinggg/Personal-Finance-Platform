using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Exception;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Transaction.UpdateTransaction
{
    public class UpdateTransactionHandler(
        ITransactionRepository transactionRepository,
             IWalletRepository walletRepository,
             IUnitOfWork unitOfWork
        ) : IRequestHandler<UpdateTransactionCommand, Result<UpdateTransactionResult>>
    {

        private readonly ITransactionRepository _transactionRepo = transactionRepository;
        private readonly IWalletRepository _walletRepo = walletRepository;
        private readonly IUnitOfWork _uow = unitOfWork;

        public async Task<Result<UpdateTransactionResult>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            bool transactionStarted = false;
            try
            {
                var transaction = await _transactionRepo.FindByIdForUserAsync(request.TransactionId, request.UserId, cancellationToken);
                if (transaction is null)
                    return Result<UpdateTransactionResult>.NotFound("Transaction doesn't exist.");

                var wallet = await _walletRepo.GetByIdAsync(transaction.WalletId, cancellationToken);
                if(wallet is null)
                    return Result<UpdateTransactionResult>.Error("Invalid wallet. Please make sure you choose the right wallet.");

                await _uow.BeginAsync(cancellationToken);
                transactionStarted = true;

                //Revert back old transaction from wallet
                wallet.RevertTransaction(transaction);

                //Update transaction
                transaction.EditTransaction(wallet.Id, request.Amount, request.Description, request.TransactionType, request.TransactionAt);

                //Apply new transaction to wallet
                wallet.ApplyTransaction(transaction);

                wallet.EnsureValidBalance();

                await _uow.CommitAsync(cancellationToken);

                return Result<UpdateTransactionResult>.Success(new UpdateTransactionResult(transaction.Id));
            }
            catch (DomainException ex)
            {
                //This is domain exception, which is to check INVARIANT
                switch (ex.Category)
                {
                    case FailureCategory.Invariant:
                        return Result<UpdateTransactionResult>.Invalid(ex.Message);
                    case FailureCategory.Policy or FailureCategory.State:
                        return Result<UpdateTransactionResult>.Fail(ex.Message);
                    default:
                        return Result<UpdateTransactionResult>.Error("Unhandled domain exception.");
                }
            }
            catch
            {
                if(transactionStarted) await _uow.RollbackAsync(cancellationToken);

                return Result<UpdateTransactionResult>.Error("An unexpected error occurred..");
            }
        }
    }
}
