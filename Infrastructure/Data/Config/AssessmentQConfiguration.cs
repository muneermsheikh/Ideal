using Core.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class AssessmentQConfiguration : IEntityTypeConfiguration<AssessmentQ>
    {
        public void Configure(EntityTypeBuilder<AssessmentQ> builder)
        {
            builder.Property(x => x.Question).IsRequired().HasMaxLength(150);
            builder.HasIndex("EnquiryItemId");
        }
    }
}