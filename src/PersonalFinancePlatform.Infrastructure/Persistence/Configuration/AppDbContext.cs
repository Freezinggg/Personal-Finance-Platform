using Microsoft.EntityFrameworkCore;
using PersonalFinancePlatform.Domain.User.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Configuration
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
