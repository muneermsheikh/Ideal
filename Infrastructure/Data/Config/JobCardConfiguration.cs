using Core.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class JobCardConfiguration : IEntityTypeConfiguration<JobCard>
    {
        public void Configure(EntityTypeBuilder<JobCard> builder)
        {
            builder.Property(x => x.JobCardDate).IsRequired();
            builder.Property(x => x.OkToConsider).IsRequired();
            builder.Property(x => x.OkToForwardCVToClient).IsRequired();
            builder.Property(x => x.PPInPossession).IsRequired();
            builder.Property(x => x.JobCardDate).IsRequired();
            builder.Property(x => x.PPIsValid).IsRequired();
            builder.Property(x => x.RemunerationAcceptable).IsRequired();
            builder.Property(x => x.ServiceChargesAcceptable).IsRequired();
            builder.Property(x => x.SuspiciousCandidate).IsRequired();
            builder.Property(x => x.WillingToEmigrate).IsRequired();
            builder.Property(x => x.WillingToTravelWithinTwoWeeksOfSelection).IsRequired();

            builder.HasIndex("EnquiryItemId", "CandidateId").IsUnique();
        }
    }
}