using System;
using System.Linq;
using System.Reflection;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Entities.Processing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class ATSContext : DbContext
    {
        public ATSContext(DbContextOptions<ATSContext> options) : base(options)
        {
        }

        public DbSet<Assessment> Assessments {get; set; }
        public DbSet<AssessmentItem> AssessmentItems {get; set; }
        public DbSet<Attachment> Attachments {get; set;}
        public DbSet<AssessmentQ> AssessmentQs {get; set; }
        public DbSet<AssessmentQBank> AssessmentQsBank {get; set;}
        public DbSet<Category> Categories {get; set; }
        public DbSet<Candidate> Candidates {get; set; }
        public DbSet<CandidateCategory> CandidateCategories {get; set; }
        public DbSet<ContractReviewItem> ContractReviewItems {get; set; }
        public DbSet<CustomerAddress> CustomerAddresses {get; set; }
        public DbSet<Customer> Customers {get; set; }
        public DbSet<CustomerOfficial> CustomerOfficials {get; set; }
        public DbSet<CVEvaluation> CVEvaluations {get; set; }
        public DbSet<CVForward> CVForwards {get; set; }
        public DbSet<CVForwardItem> CVForwardItems {get; set; }
        public DbSet<CVRef> CVRefs {get; set; }
        //public DbSet<DomainSub> DomainSubs {get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods {get; set; }
        public DbSet<DLForwardToHR> DLForwardToHR {get; set; }
        public DbSet<Employee> Employees {get; set; }
        public DbSet<Emolument> Emoluments{get; set;}
        public DbSet<EnquiryForwarded> EnquiryForwards {get; set; }
        public DbSet<Enquiry> Enquiries {get; set; }
        public DbSet<EnquiryItem> EnquiryItems {get; set; }
        public DbSet<EnquiryItemAssessmentQ> EnquiryItemAssessmentQs{get; set;}
        public DbSet<Grade> Grades {get; set; }
        public DbSet<HRSkillClaim> HRSkillClaims {get; set; }
        public DbSet<IndustryType> IndustryTypes {get; set; }
        public DbSet<JobCard> JobCards {get; set; }
        
        public DbSet<JobDesc> JobDescriptions {get; set; }
        public DbSet<Process> Processes {get; set; }
        public DbSet<ProcessStatus> ProcessStatuses {get; set;}
        
        public DbSet<Remuneration> Remunerations{get; set; }
        public DbSet<Role> Roles {get; set; }

        public DbSet<SelDecision> SelDecisions {get; set; }   
        public DbSet<Skill> Skills {get; set; }     
        public DbSet<SkillLevel> SkillLevels {get; set; }
        public DbSet<Source> Sources {get; set; }
        public DbSet<SourceGroup> SourceGroups {get; set; }
        public DbSet<ToDo> ToDos {get; set; }
        public DbSet<TaskItem> TaskItems {get; set; }
        public DbSet<Travel> Travels {get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityframeworkCore.Sqlite")
            {
                foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var dateProperties = entityType.ClrType.GetProperties()
                        .Where(x => x.PropertyType == typeof(DateTime));
                    foreach(var property in dateProperties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }

            // Sqlite does not support Sequence, therefore when using MySql or SQLServer, use following
            // to initiate Enquiry No. field
        /*
            modelBuilder.ForSqlServerHasSequence<int>("DBSequence")
                .StartsAt(1000).IncrementsBy(1);
            modelBuilder.Entity<Enquiry>.Property(x => x.EnquiryNo)
                .HasDefaultValueSql("NEXT VALUE FOR DBSequence");
        */
        }
    }
}