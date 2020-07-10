using Core.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class EnquiryForwardedConfiguration : IEntityTypeConfiguration<EnquiryForwarded>
    {
        public void Configure(EntityTypeBuilder<EnquiryForwarded> builder)
        {
            builder.Property(x => x.CustomerOfficialId).IsRequired();
            builder.HasIndex("CustomerOfficialId");
            builder.HasIndex("EnquiryItemId");
            builder.Property(x => x.ForwardedOn).IsRequired();
        }
    }
}