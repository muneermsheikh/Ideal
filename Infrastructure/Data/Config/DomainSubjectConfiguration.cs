using Core.Entities.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class DomainSubjectConfiguration : IEntityTypeConfiguration<DomainSubject>
    {
        public void Configure(EntityTypeBuilder<DomainSubject> builder)
        {
            builder.Property(x => x.Domain).IsRequired().HasMaxLength(100);
            builder.HasIndex("Domain").IsUnique();
        }
    }
}