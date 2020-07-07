using System.Reflection;
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

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}