using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface IUnitOfWork
    {
        Task BeginAsync(CancellationToken ct);
        Task CommitAsync(CancellationToken ct);
        Task RollbackAsync(CancellationToken ct);
    }
}
