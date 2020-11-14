using Core.Entities.Masters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.Designation).IsRequired();
            builder.Property("DateOfJoining").IsRequired();
            builder.HasMany(o => o.Addresses).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.Skills).WithOne().OnDelete(DeleteBehavior.Cascade);
            // builder.OwnsOne(o => o.Person, a => {WIthOwner();});
        }
    }
}