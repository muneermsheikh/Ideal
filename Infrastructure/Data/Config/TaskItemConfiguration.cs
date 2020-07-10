using Core.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.Property(x => x.TaskId).IsRequired();
            builder.Property(x => x.TransDate).IsRequired();
            builder.Property(x => x.TransactionDetail).IsRequired();
        }
    }
}