using Core.Entities.EnquiryAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class JobDescConfiguration : IEntityTypeConfiguration<JobDesc>
    {
        public void Configure(EntityTypeBuilder<JobDesc> builder)
        {
            builder.Property(x => x.JobDescription).IsRequired();
            // builder.HasIndex(x => x.EnquiryItemId).IsUnique();
        }
    }
}