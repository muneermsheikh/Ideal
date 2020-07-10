using System;
using Core.Entities.Admin;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CityName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.KnownAs).IsRequired().HasMaxLength(15);

            builder.Property(s => s.CustomerType)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumCustomerType) Enum.Parse(typeof(enumCustomerType), o)
                );

            builder.Property(s => s.CustomerStatus)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumCustomerStatus) Enum.Parse(typeof(enumCustomerStatus), o)
                );
            
            builder.HasMany(x => x.CustomerAddresses).WithOne()
                .HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(o => o.CustomerOfficials).WithOne()
                .HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}