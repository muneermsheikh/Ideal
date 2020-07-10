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
            builder.Property("DOJ").IsRequired();
            builder.OwnsOne(o => o.Person, a => 
            {
                a.WithOwner();
            });
            
            builder.OwnsOne(o => o.EmployeeAddress, a => 
            {
                a.WithOwner();
            });
        }
    }
}