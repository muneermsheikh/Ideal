using Core.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AssessmentItemConfiguration : IEntityTypeConfiguration<AssessmentItem>
    {
        public void Configure(EntityTypeBuilder<AssessmentItem> builder)
        {
            builder.Property(x => x.AssessmentId).IsRequired();
            builder.Property(x => x.Question).IsRequired();
            builder.HasIndex("QuestionNo", "AssessmentId").IsUnique();
        }
    }
}