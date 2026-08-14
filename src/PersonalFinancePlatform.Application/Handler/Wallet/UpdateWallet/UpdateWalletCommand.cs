using MediatR;
using PersonalFinancePlatform.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Wallet.UpdateWallet
{
    public sealed class UpdateWalletCommand : IRequest<Result<bool>>
    {
        public Guid UserId { get; }
        public Guid WalletId { get; }
        public string WalletName { get; }

        public UpdateWalletCommand(Guid userId, Guid walletId, string walletName)
        {
            UserId = userId;
            WalletId = walletId;
            WalletName = walletName;
        }
    }
}
