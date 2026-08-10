using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Transaction.UpdateTransaction
{
    public sealed class UpdateTransactionValidator
    : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.TransactionType)
                .IsInEnum();

            RuleFor(x => x.TransactionAt)
                .NotEmpty();
        }
    }
}
