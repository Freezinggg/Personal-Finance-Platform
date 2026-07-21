using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(Email email, CancellationToken cancellationToken);

        Task AddAsync(User user, CancellationToken cancellationToken);
    }
}
