using MediatR;
using PersonalFinancePlatform.Application.Common;
using PersonalFinancePlatform.Application.Handler.Auth.RegisterUser;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Application.Interfaces.Security;
using PersonalFinancePlatform.Domain.Exception;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static PersonalFinancePlatform.Domain.Exception.DomainException;

namespace PersonalFinancePlatform.Application.Handler.Transaction.RecordTransaction
{
    public class RecordTransactionHandler(
             ITransactionRepository transactionRepository,
             IWalletRepository walletRepository,
             IUnitOfWork unitOfWork
        ) : IRequestHandler<RecordTransactionCommand, Result<RecordTransactionResult>>
    {

        private readonly ITransactionRepository _transactionRepo = transactionRepository;
        private readonly IWalletRepository _walletRepo = walletRepository;
        private readonly IUnitOfWork _uow = unitOfWork;

        public async Task<Result<RecordTransactionResult>> Handle(RecordTransactionCommand request, CancellationToken cancellationToken)
        {
            bool transactionStarted = false;
            try
            {
                DateTime now = DateTime.UtcNow;
                //1. Validate request, but mostly its inside domain, no global invariant/business yet.

                //2. Load wallet based on request.WalletId
                var wallet = await _walletRepo.GetByIdAsync(request.WalletId, cancellationToken);
                if (wallet is null)
                    return Result<RecordTransactionResult>.Invalid("Invalid wallet. Please make sure you choose the right wallet.");

                //Create transaction
                Domain.Transaction.Entities.Transaction transaction =
                    new(request.WalletId, request.Amount, request.Description, request.TransactionType, request.TransactionAt, now);

                //3. Start of UoW
                await _uow.BeginAsync(cancellationToken);
                transactionStarted = true;

                //persist transaction
                _transactionRepo.Add(transaction);

                //Apply transaction and adjust wallet's balance based on transaction
                wallet.ApplyTransaction(transaction);

                //Commit UoW
                await _uow.CommitAsync(cancellationToken);

                return Result<RecordTransactionResult>.Success(new RecordTransactionResult(transaction.Id));

            }
            catch (DomainException ex)
            {
                //This is domain exception, which is to check INVARIANT
                switch (ex.Category)
                {
                    case FailureCategory.Invariant:
                        return Result<RecordTransactionResult>.Invalid(ex.Message);
                    case FailureCategory.Policy or FailureCategory.State:
                        return Result<RecordTransactionResult>.Fail(ex.Message);
                    default:
                        return Result<RecordTransactionResult>.Error("Unhandled domain exception.");
                }
            }
            catch
            {
                if(transactionStarted) await _uow.RollbackAsync(cancellationToken);

                return Result<RecordTransactionResult>.Error("An unexpected error occurred..");
            }

        }
    }
}
