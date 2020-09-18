using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Admin
{
    [NotMapped]
    public class CVForwardDto
    {
        public CVForwardDto()
        {
        }

/*
        public CVForwardDto(int id, DateTime dateForwarded, string customerName, string officialName, 
            string officialEmailId, string mailSentRef, List<CVForwardItemDto> cVForwardItems)
        {
            Id = id;
            DateForwarded = dateForwarded;
            CustomerName = customerName;
            OfficialName = officialName;
            OfficialEmailId = officialEmailId;
            MailSentRef = mailSentRef;
            CVForwardItems = cVForwardItems;
        }
*/
        public int Id {get; set; }
        public DateTime DateForwarded {get; set; }
        public string CustomerName {get; set;}
        public string OfficialName {get; set;}
        public string OfficialEmailId {get; set;}
        public bool IncludeSalary {get; set;}
        public bool IncludeGrade {get; set; }
        public bool IncludePhoto {get; set; }
        public string MailSentRef {get; set;}

        public int SentMessageToClient {get; set;}

        public List<CVForwardItemDto> CVForwardItems {get; set;}
    }

    [NotMapped]
    public class CVForwardItemDto
    {
        public CVForwardItemDto()
        {
        }

/*
        public CVForwardItemDto(int srNo, string categoryRef, int applicationNo, 
            string candidateName, string pPNo)
        {
            SrNo = srNo;
            CategoryRef = categoryRef;
            ApplicationNo = applicationNo;
            CandidateName = candidateName;
        }
*/
        public int SrNo {get; set; }
        public string CategoryRef {get; set;}
        public int ApplicationNo {get; set; }
        public string CandidateName {get; set;}
        public string PPNo {get; set; }
    }
}