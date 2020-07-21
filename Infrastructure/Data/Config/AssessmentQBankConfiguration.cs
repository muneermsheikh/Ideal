using Core.Entities.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AssessmentQBankConfiguration : IEntityTypeConfiguration<AssessmentQBank>
    {
        public void Configure(EntityTypeBuilder<AssessmentQBank> builder)
        {
            // builder.HasIndex(x => x.DomainSubj);
            builder.HasIndex(x => x.AssessmentParameter);
            builder.HasIndex("DomainSubId", "SrNo").IsUnique();
            builder.Property(x => x.AssessmentParameter).IsRequired();
            builder.Property(x => x.DomainSubId).IsRequired();
            builder.Property(x => x.Question).IsRequired();
        }
    }
}