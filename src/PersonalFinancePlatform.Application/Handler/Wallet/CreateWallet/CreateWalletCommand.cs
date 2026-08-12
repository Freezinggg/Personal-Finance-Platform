using MediatR;
using PersonalFinancePlatform.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Handler.Wallet.CreateWallet
{
    public sealed class CreateWalletCommand : IRequest<Result<Guid>>
    {
        public Guid UserId { get; }
        public string WalletName { get; }

        public CreateWalletCommand(Guid userId, string walletName)
        {
            UserId = userId;
            WalletName = walletName;
        }
    }
}
