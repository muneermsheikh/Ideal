using System;
using Core.Entities.Admin;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities.Masters;

namespace Infrastructure.Data.Config
{
    public class CustomerIndustryTypeConfiguration : IEntityTypeConfiguration<CustomerIndustryType>
    {
        public void Configure(EntityTypeBuilder<CustomerIndustryType> builder)
        {
            builder.HasIndex("CustomerId", "IndustryTypeId").IsUnique();
            
            // ** define foreign key to industry Type
            
        }
    }
}