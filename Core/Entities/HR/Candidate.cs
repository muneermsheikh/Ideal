using System;
using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.HR
{
    public class Candidate: BaseEntity
    {
        public Candidate()
        {
        }

        public int ApplicationNo { get; set; }
        public DateTime ApplicationDated { get; set; }=DateTime.Now;
        [Required]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        [Required]
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string PPNo { get; set; }
        public bool ECNR {get; set;}=false;
        public string AadharNo { get; set; }
        public DateTime DOB { get; set; }
        
        public int? ReferredById {get; set;}
        public string email {get; set; }
        public string ContactPreference {get; set;}
        public virtual CandidateAddress CandidateAddress { get; set; }
        public virtual List<EnquiryItem> ReferredToEnquiryItems { get; set; }
        public enumCandidateStatus CandidateStatus { get; set; } = enumCandidateStatus.Available;
        public virtual Employee LastStatusUpdatedBy { get; set; }
        
        public DateTime LastStatusUpdatedOn { get; set; } = DateTime.Now.Date;
        public DateTime AddedOn {get; set;} = DateTime.Now.Date;
        public virtual List<Attachment> Attachments {get; set; }

        public virtual List<CandidateCategory> CandidateCategories {get; set; }
        public string FullName {get {return FirstName + " " + FamilyName;} }
    }
}