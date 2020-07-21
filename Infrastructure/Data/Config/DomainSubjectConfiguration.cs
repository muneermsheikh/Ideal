using Core.Entities.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class DomainSubjectConfiguration : IEntityTypeConfiguration<DomainSub>
    {
        public void Configure(EntityTypeBuilder<DomainSub> builder)
        {
            builder.Property(x => x.DomainSubName).IsRequired().HasMaxLength(100);
            builder.HasIndex("DomainSubName").IsUnique();
        }
    }
}