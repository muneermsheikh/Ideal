using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class EnquiryConfiguration : IEntityTypeConfiguration<Enquiry>
    {
        public void Configure(EntityTypeBuilder<Enquiry> builder)
        {
            //builder.OwnsOne( o => o.ShipToAddress, a => {a.WithOwner();});

            builder.Property(x => x.EnquiryStatus).HasConversion(
                o => o.ToString(),
                o => (enumEnquiryStatus) Enum.Parse(typeof(enumEnquiryStatus), o)
            );
            
            // use seqence object in database
            // builder.Property(x => x.EnquiryNo).ValueGeneratedOnAdd();
            builder.HasMany(x => x.EnquiryItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}