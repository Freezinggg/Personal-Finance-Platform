using Microsoft.EntityFrameworkCore.Storage;
using PersonalFinancePlatform.Application.Interfaces.Persistence;
using PersonalFinancePlatform.Infrastructure.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.UnitOfWork
{
    public sealed class EFUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IDbContextTransaction? _tx;

        public EFUnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public async Task BeginAsync(CancellationToken ct)
        {
            _tx = await _db.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct)
        {
            await _db.SaveChangesAsync(ct);
            await _tx!.CommitAsync(ct);
        }

        public async Task RollbackAsync(CancellationToken ct)
        {
            await _tx!.RollbackAsync(ct);
        }
    }
}
