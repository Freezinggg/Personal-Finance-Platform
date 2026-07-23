using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Configuration
{
    public sealed class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {

            builder.ToTable("Wallets");

            builder.HasKey(x => x.Id);

            builder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.WalletName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Balance)
                .HasPrecision(18, 2);

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}
