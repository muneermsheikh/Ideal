using System;
using Core.Entities.Processing;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ProcessingConfiguration : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            builder.Property(x => x.CVRefId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasIndex("CVRefId");
            builder.HasIndex("CVRefId", "Status").IsUnique();
            
            builder.Property(s => s.Status)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumProcessingStatus) Enum.Parse(typeof(enumProcessingStatus), o)
                );
        }
    }
}