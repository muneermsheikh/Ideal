using System;
using Core.Entities.HR;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.HasIndex(x => x.EnquiryItemId);
            builder.Property(x => x.CandidateId).IsRequired();
            builder.HasOne(x => x.Candidate).WithMany().HasForeignKey(x => x.CandidateId);
            builder.HasIndex(x => x.CandidateId);
            builder.HasMany(x => x.AssessmentItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex("EnquiryItemId", "CandidateId").IsUnique();

            builder.Property(s => s.Result)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumAssessmentResult) Enum.Parse(typeof(enumAssessmentResult), o)
                );
        }
    }
}