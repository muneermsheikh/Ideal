using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class EnquiryItemConfiguration : IEntityTypeConfiguration<EnquiryItem>
    {
        public void Configure(EntityTypeBuilder<EnquiryItem> builder)
        {
            builder.OwnsOne(i => i.ItemOrdered, io => {io.WithOwner();});
        
            builder.HasIndex("EnquiryId", "CategoryItemId").IsUnique();
        
        }
    }
}