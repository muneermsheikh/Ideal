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
            
            // use seqence object in database
            // builder.Property(x => x.EnquiryNo).ValueGeneratedOnAdd();
            builder.HasMany(x => x.EnquiryItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}