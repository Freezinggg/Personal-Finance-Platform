using MediatR;
using PersonalFinancePlatform.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Wallet.DeleteWallet
{
    public sealed class DeleteWalletCommand : IRequest<Result<bool>>
    {
        public Guid WalletId { get; }
        public Guid UserId { get; }
        public DeleteWalletCommand(Guid walletId, Guid userId)
        {
            WalletId = walletId;
            UserId = userId;
        }
    }
}
