using System;
using Core.Entities.HR;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.Property( x => x.ApplicationNo).IsRequired();
            builder.HasIndex(x => x.ApplicationNo).IsUnique();

            // Following returns the string value of the enumeration Type
            builder.Property(s => s.CandidateStatus)
                .HasConversion(
                    o => o.ToString(),
                    o => (enumCandidateStatus) Enum.Parse(typeof(enumCandidateStatus), o)
                );

            builder.OwnsOne(o => o.CandidateAddress, a => 
            {
                a.WithOwner();
            });

            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.FamilyName).IsRequired();
            builder.Property(x => x.Gender).IsRequired();
            builder.Property(x => x.PPNo).IsRequired();      // TO DO - consider implementing this in stage 2, in order to have the candidate onboard first
            builder.HasIndex(x => x.PPNo).IsUnique();

            // when candidate is deleted, it will delete all its many side entities
            builder.HasMany(o => o.CandidateCategories).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.ReferredToEnquiryItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(o => o.Attachments).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}