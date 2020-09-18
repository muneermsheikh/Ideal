using Core.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SelDecisionConfiguration : IEntityTypeConfiguration<SelDecision>
    {
        public void Configure(EntityTypeBuilder<SelDecision> builder)
        {
            builder.Property(x => x.CandidateId).IsRequired();
            builder.Property(x => x.CVRefID).IsRequired();
            builder.Property(x => x.SelectionDate).IsRequired();
            builder.Property(x => x.SelectionResult).IsRequired();
            builder.HasIndex(x => x.CVRefID).IsUnique();

        }
    }
}