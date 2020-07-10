using Core.Entities.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(75);
            builder.Property(x => x.SkillLevelId).IsRequired();
            builder.Property(x => x.IndustryTypeId).IsRequired();
        
            // builder.Property(x => x.Id).HasColumnType("decimal(18,2)");
            // builder.HasOne(x => x.IndustryType).WithMany().HasForeignKey(x => x.IndustryTypeId);
            // builder.HasOne(x => x.SkillLevel).WithMany().HasForeignKey(x => x.SkillLevelId);
            builder.HasIndex("Name", "SkillLevelId", "IndustryTypeId").IsUnique();
        }
    }
}