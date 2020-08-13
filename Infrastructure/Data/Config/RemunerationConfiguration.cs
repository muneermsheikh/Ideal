using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class RemunerationConfiguration : IEntityTypeConfiguration<Remuneration>
    {
        public void Configure(EntityTypeBuilder<Remuneration> builder)
        {
           /* builder.Property(x => x.Housing).IsRequired();
            builder.Property(x => x.Food).IsRequired();
            builder.Property(x => x.Transport).IsRequired();
            builder.Property(x => x.SalaryMin).IsRequired();
            builder.Property(x => x.SalaryCurrency).IsRequired();.
            builder.Property(x => x.LeaveAvailableAfterHowmanyMonths).IsRequired();
           */ builder.Property( x => x.LeaveEntitlementPerYear).IsRequired();
            builder.HasIndex(x => x.EnquiryItemId).IsUnique();

            builder.Property(s => s.Housing)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumProvision) Enum.Parse(typeof(enumProvision), o)
                );

            builder.Property(s => s.Food)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumProvision) Enum.Parse(typeof(enumProvision), o)
                );
            builder.Property(s => s.Transport)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumProvision) Enum.Parse(typeof(enumProvision), o)
                );
            
        }
    }
}