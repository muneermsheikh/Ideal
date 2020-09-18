using Core.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CVRefConfiguration : IEntityTypeConfiguration<CVRef>
    {
        public void Configure(EntityTypeBuilder<CVRef> builder)
        {
             builder.HasMany(o => o.ProcessTransactions).WithOne().OnDelete(DeleteBehavior.Cascade);
             builder.HasOne(o => o.SelDecision).WithOne().OnDelete(DeleteBehavior.Cascade);
             
        }
    }
}