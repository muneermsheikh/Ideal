using Core.Entities.Admin;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class EmolumentsConfig : IEntityTypeConfiguration<Emolument>
    {
        public void Configure(EntityTypeBuilder<Emolument> builder)
        {
             builder.Property(x => x.SalaryCurrency).IsRequired();
             builder.Property(x => x.BasicSalary).IsRequired();
             builder.Property(x => x.ContractPeriodInMonths).IsRequired().HasDefaultValue(24);
             builder.Property(x => x.CVRefId).IsRequired();
             builder.Property(x => x.Food).IsRequired().HasDefaultValue(enumProvision.NotProvided);
             builder.Property(x => x.Housing).IsRequired().HasDefaultValue(enumProvision.ProvidedFree);
             builder.Property(x => x.WeeklyWorkHours).IsRequired().HasDefaultValue(48);
             builder.Property(x => x.AirportOfDestination).IsRequired();
             builder.Property(x => x.Transport).IsRequired().HasDefaultValue(enumProvision.ProvidedFree);
             builder.Property(x => x.LeaveEntitlementAfterMonths).IsRequired().HasDefaultValue(23);
             builder.HasIndex("CVRefId").IsUnique();
        }
    }
}