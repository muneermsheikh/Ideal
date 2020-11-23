using System;
using Core.Entities.HR;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CVEvaluationConfiguration : IEntityTypeConfiguration<CVEvaluation>
    {
        public void Configure(EntityTypeBuilder<CVEvaluation> builder)
        {
            builder.Property(x => x.CandidateId).IsRequired();
            builder.Property(x => x.EnquiryItemId).IsRequired();
            builder.Property(x => x.HRExecutiveId).IsRequired();
    /*
            builder.Property(s => s.HRSupReviewResult)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumItemReviewStatus) Enum.Parse(typeof(enumItemReviewStatus), o)
                );
            builder.Property(s => s.HRMReviewResult)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumItemReviewStatus) Enum.Parse(typeof(enumItemReviewStatus), o)
                );
    */
        }
    }
}