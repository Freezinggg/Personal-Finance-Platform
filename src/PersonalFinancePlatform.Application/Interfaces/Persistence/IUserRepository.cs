using PersonalFinancePlatform.Domain.User.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Application.Interfaces.Persistence
{
    public interface IUserRepository
    {
        Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);

        Task AddAsync(User user, CancellationToken cancellationToken);
    }
}
