using System;
using Core.Entities.Processing;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProcessingConfiguration : IEntityTypeConfiguration<Processing>
    {
        public void Configure(EntityTypeBuilder<Processing> builder)
        {
            builder.Property(x => x.CVRefId).IsRequired();
            builder.HasIndex(x => x.CVRefId);
            builder.Property(x => x.Status).IsRequired();

            builder.Property(s => s.Status)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumProcessingStatus) Enum.Parse(typeof(enumProcessingStatus), o)
                );
        }
    }
}