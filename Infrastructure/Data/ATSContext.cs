using System.Reflection;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ATSContext : DbContext
    {
        public ATSContext(DbContextOptions<ATSContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories {get; set; }
        public DbSet<IndustryType> IndustryTypes {get; set; }
        public DbSet<SkillLevel> SkillLevels {get; set; }

        public DbSet<Enquiry> Enquiries {get; set; }
        public DbSet<EnquiryItem> EnquiryItems {get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods {get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}