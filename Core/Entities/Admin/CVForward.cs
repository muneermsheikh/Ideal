using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Admin
{
    public class CVForward: BaseEntity
    {
        public CVForward()
        {
        }

        public CVForward(int customerid, int customerofficialid)
        {
            CustomerId=customerid;
            CustomerOfficialId=customerofficialid;
        }

        public CVForward(int id, DateTime dateForwarded, string remarks, int customerId, int customerOfficialId, string officialEmailId, bool includeSalary, bool includeGrade, bool includePhoto, List<CVForwardItem> cVForwardItems)
        {
            Id = id;
            DateForwarded = dateForwarded;
            Remarks = remarks;
            CustomerId = customerId;
            CustomerOfficialId = customerOfficialId;
            OfficialEmailId = officialEmailId;
            IncludeSalary = includeSalary;
            IncludeGrade = includeGrade;
            IncludePhoto = includePhoto;
            CVForwardItems = cVForwardItems;
        }

        public DateTime DateForwarded {get; set; }
        public string Remarks {get; set; }
        public int CustomerId {get; set;}
        public int CustomerOfficialId {get; set;}
        public string OfficialEmailId {get; set;}
        public bool IncludeSalary {get; set;}
        public bool IncludeGrade {get; set; }
        public bool IncludePhoto {get; set; }
        public string MailSentRef {get; set;}

        public int SentMessageToClient {get; set;}

        public List<CVForwardItem> CVForwardItems {get; set;}
        public Customer Customer {get; set;}
    }

    public class CVForwardItem: BaseEntity
    {
        public CVForwardItem()
        {
        }

        public CVForwardItem(int srno, int appno, int enquiryItemId, int enquiryId, int candidateid, int id)
        {
            SrNo = srno;
            ApplicationNo = appno;
            EnquiryItemId = enquiryItemId;
            EnquiryId = enquiryId;
            CandidateId = candidateid;
            CVRefId=id;
        }

        public int CVForwardId {get; set; }
        public int SrNo {get; set;}
        public int EnquiryId {get; set;}
        public int EnquiryItemId {get; set; }
        public int CandidateId {get; set;}
        public int ApplicationNo {get; set;}
        //public string Grade {get; set;}
        //public string salaryexpectation {get; set;}
        //public string photoUrl {get; set; }
        public int CVRefId {get; set; }
    }
}