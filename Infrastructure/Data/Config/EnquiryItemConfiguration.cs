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
        
            builder.Property(x => x.Status).HasConversion(
                o => o.ToString(),
                o => (enumItemReviewStatus) Enum.Parse(typeof(enumItemReviewStatus), o)
            );
        
            builder.Property(x => x.Food).HasConversion(
                o => o.ToString(),
                o => (enumProvision) Enum.Parse(typeof(enumProvision), o)
            );
            builder.Property(x => x.Housing).HasConversion(
                o => o.ToString(),
                o => (enumProvision) Enum.Parse(typeof(enumProvision), o)
            );
            builder.Property(x => x.Transport).HasConversion(
                o => o.ToString(),
                o => (enumProvision) Enum.Parse(typeof(enumProvision), o)
            );

        }
    }
}