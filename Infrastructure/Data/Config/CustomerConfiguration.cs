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
            builder.Property(x => x.City).IsRequired().HasMaxLength(50);
            builder.Property(x => x.KnownAs).IsRequired().HasMaxLength(15);
            builder.HasMany(o => o.CustomerOfficials).WithOne()
                .HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany( o => o.CustomerIndustryTypes).WithOne()
                .HasForeignKey( o => o.CustomerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}