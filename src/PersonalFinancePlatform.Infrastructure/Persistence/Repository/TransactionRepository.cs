using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Domain.Transaction.Entities;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Repository
{
    public sealed class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public void Add(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
        }
    }
}
