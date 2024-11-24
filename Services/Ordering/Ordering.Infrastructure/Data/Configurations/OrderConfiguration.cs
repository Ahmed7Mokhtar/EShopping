using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(m => m.Id).HasMaxLength(100)
                .ValueGeneratedOnAdd();

            builder.Property(m => m.TotalPrice).HasPrecision(18, 3);

            builder.Property(m => m.UserName).HasMaxLength(500);
            builder.Property(m => m.FirstName).HasMaxLength(200);
            builder.Property(m => m.LastName).HasMaxLength(200);
            builder.Property(m => m.Email).HasMaxLength(500);
            builder.Property(m => m.Country).HasMaxLength(200);
            builder.Property(m => m.AddressLine).HasMaxLength(500);
            builder.Property(m => m.State).HasMaxLength(200);
            builder.Property(m => m.Zipcode).HasMaxLength(200);
            builder.Property(m => m.CardName).HasMaxLength(200);
            builder.Property(m => m.CardNumber).HasMaxLength(200);
            builder.Property(m => m.Expiration).HasMaxLength(100);
            builder.Property(m => m.Cvv).HasMaxLength(100);

            builder.HasIndex(m => m.UserName);
        }
    }
}
