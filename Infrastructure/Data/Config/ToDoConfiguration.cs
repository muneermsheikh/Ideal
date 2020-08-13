using System;
using Core.Entities.Admin;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ToDoConfiguration : IEntityTypeConfiguration<ToDo>
    {
        public void Configure(EntityTypeBuilder<ToDo> builder)
        {
            // builder.Property(x => x.EnquiryItemId).IsRequired();
            builder.HasOne(x => x.Owner).WithMany().HasForeignKey(x => x.OwnerId);
            builder.HasOne(x => x.AssignedTo).WithMany().HasForeignKey(x => x.AssignedToId);
            builder.Property(x => x.TaskDate).IsRequired();
            builder.Property(x => x.CompleteBy).IsRequired();
            builder.Property(x => x.TaskType).IsRequired().HasDefaultValue(enumTaskType.Administrative);
            builder.Property(x => x.TaskDescription).IsRequired().HasMaxLength(250);
            builder.Property(x => x.TaskStatus).IsRequired();
            builder.HasMany(x => x.TaskItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex("AssignedToId");
            builder.HasIndex("OwnerId");
            builder.HasIndex("TaskType");
            
            // ** require index for assigned to and owner???
            builder.Property(s => s.TaskType)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumTaskType) Enum.Parse(typeof(enumTaskType), o)
                );
                
             builder.Property(s => s.TaskStatus)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumTaskStatus) Enum.Parse(typeof(enumTaskStatus), o)
                );
        }
    }
}