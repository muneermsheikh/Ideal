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

        public CVForward(int id, DateTime dateForwarded, string remarks, int enquiryId, int customerId, int customerOfficialId, string officialEmailId, bool includeSalary, bool includeGrade, bool includePhoto, List<CVForwardItem> cVForwardItems)
        {
            Id = id;
            DateForwarded = dateForwarded;
            Remarks = remarks;
            EnquiryId = enquiryId;
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
        public int EnquiryId {get; set; }
        public int CustomerId {get; set;}
        public int CustomerOfficialId {get; set;}
        public string OfficialEmailId {get; set;}
        public bool IncludeSalary {get; set;}
        public bool IncludeGrade {get; set; }
        public bool IncludePhoto {get; set; }

        public bool SendMessageToClient {get; set;}

        public List<CVForwardItem> CVForwardItems {get; set;}
    }

    public class CVForwardItem: BaseEntity
    {
        public CVForwardItem()
        {
        }

        public CVForwardItem(int enquiryItemId, List<CVRef> listCVRef)
        {
            EnquiryItemId=enquiryItemId;
            CVsRef = listCVRef;
        }

        public int EnquiryItemId {get; set; }
        public List<CVRef> CVsRef {get; set; }
    }
}