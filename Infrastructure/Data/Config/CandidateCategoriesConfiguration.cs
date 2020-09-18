using Core.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CandidateCategoriesConfiguration: IEntityTypeConfiguration<CandidateCategory>
    {
        public void Configure(EntityTypeBuilder<CandidateCategory> builder)
        {
            //builder.HasMany(x => x.Assessment).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex("CatId", "CandId").IsUnique();
        }
    }
}