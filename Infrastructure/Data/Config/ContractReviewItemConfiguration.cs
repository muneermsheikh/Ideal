using System;
using Core.Entities.Admin;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ContractReviewItemConfiguration : IEntityTypeConfiguration<ContractReviewItem>
    {
        public void Configure(EntityTypeBuilder<ContractReviewItem> builder)
        {
            builder.Property(x => x.EnquiryItemId).IsRequired();
            builder.Property(x => x.TechnicallyFeasible).IsRequired();
            builder.Property(x => x.CommerciallyFeasible).IsRequired();
            builder.Property(x => x.LogisticallyFeasible).IsRequired();
            builder.Property(x => x.VisaAvailable).IsRequired();
            builder.Property(x => x.DocumentationWillBeAvailable).IsRequired();
            builder.Property(x => x.HistoricalStatusAvailable).IsRequired();
            builder.Property(x => x.SalaryOfferedFeasible).IsRequired();
            builder.Property(x => x.ServiceChargesInINR).IsRequired();
            builder.Property(x => x.Status).IsRequired();

            builder.HasIndex(x => x.EnquiryItemId).IsUnique();

            builder.Property(s => s.Status)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumItemReviewStatus) Enum.Parse(typeof(enumItemReviewStatus), o)
                );
        }
    }
}