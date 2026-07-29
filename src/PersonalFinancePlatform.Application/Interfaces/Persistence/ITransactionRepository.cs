using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface ITransactionRepository
    {
        void Add(Domain.Transaction.Entities.Transaction transaction);
    }
}
