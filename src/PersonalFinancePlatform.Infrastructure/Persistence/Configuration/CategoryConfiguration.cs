using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinancePlatform.Domain.Category.Entities;
using PersonalFinancePlatform.Domain.User.Entities;
using PersonalFinancePlatform.Domain.Wallet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinancePlatform.Infrastructure.Persistence.Configuration
{
    public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasKey(x => x.Id);

            builder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CategoryName)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnType("citext");

            builder.HasIndex(x => new
            {
                x.OwnerId,
                x.CategoryName,
            })
           .IsUnique();

            builder.Property(x => x.IsSystem).HasDefaultValue(false);
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}
