using Core.Entities.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.SourceGroupId).IsRequired();
            builder.HasIndex("SourceGroupId", "Name").IsUnique();
        }
    }
}