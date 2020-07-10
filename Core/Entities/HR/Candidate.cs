using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Entities.Masters;

namespace Core.Entities.HR
{
    public class Candidate: BaseEntity
    {
        public int ApplicationNo { get; set; }
        public DateTime ApplicationDated { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string Gender { get; set; }
        public string PPNo { get; set; }
        
        public CandidateAddress CandidateAddress { get; set; }
        public List<Category> CandidateCategories { get; set; }
        public virtual List<EnquiryItem> ReferredToEnquiryItems { get; set; }
        public enumCandidateStatus CandidateStatus { get; set; } = enumCandidateStatus.Available;
        public virtual Employee LastStatusUpdatedBy { get; set; }
        public DateTime LastStatusUpdatedOn { get; set; } = DateTime.Now;
        public virtual List<Attachment> Attachments {get; set; }
    }
}