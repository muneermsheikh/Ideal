using System;
using Core.Entities.HR;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class HRSkillClaimsConfiguration : IEntityTypeConfiguration<HRSkillClaim>
    {
        public void Configure(EntityTypeBuilder<HRSkillClaim> builder)
        {
            builder.Property(x => x.IndustryTypeId).IsRequired();
            builder.Property(x => x.SkillLevelId).IsRequired();
            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.CategoryName).IsRequired();
        }

    }
}