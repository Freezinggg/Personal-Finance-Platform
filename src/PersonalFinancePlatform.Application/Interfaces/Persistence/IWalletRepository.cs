using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface IWalletRepository
    {
        void Add(Wallet wallet);
    }
}
