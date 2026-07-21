using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.User.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Configuration
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Email)
                .HasConversion(
                    email => email.Value,
                    value => new Email(value))
                .HasMaxLength(320)
                .IsRequired(); //map to value object of Email

            builder.HasIndex(x => x.Email)
                .IsUnique(); //no dupliation allowed for email

            builder.Property(x => x.DisplayName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}
