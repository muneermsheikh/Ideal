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
        public DateTime DateOfBirth { get; set; }
        public int ReferredById {get; set;}
        public int? SourceId {get; set;}

        public string PassportNo { get; set; }
        [Required]
        public string Ecnr {get; set;}="ECR";
        public string AadharNo { get; set; }
        public string MobileNo {get; set; }
        public string Email {get; set; }
        public string ContactPreference {get; set;}
        public string Address1 {get; set;}
        public string Address2 {get; set;}
        [Required]
        public string City {get; set;}
        public string Pin {get; set;}
        public string District {get; set;}
        public string Country {get; set;} 
        public int CandidateStatus {get; set; }
        public DateTime? LastStatusUpdatedOn { get; set; } = DateTime.Now.Date;

        public virtual Employee LastStatusUpdatedBy { get; set; }

        public virtual List<CandidateCategory> CandidateCategories {get; set; }
        public virtual List<Attachment> Attachments {get; set; }
        public virtual List<EnquiryItem> ReferredToEnquiryItems { get; set; }

        public string FullName {get {return FirstName + " " + FamilyName;} }
    }
}