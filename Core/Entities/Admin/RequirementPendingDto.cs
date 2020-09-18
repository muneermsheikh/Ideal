using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Admin
{
    [NotMapped]
    public class RequirementPendingDto
    {
        public RequirementPendingDto()
        {
        }

        public RequirementPendingDto(int enquiryno, string enqdated, string customerName, string categoryRef, int quantity, 
        int totalReferred, int totalSelected, int totalRejected, int totalUnderClientReview, int totalDeployed,
        int totalToSendToClient, string completeBy, string status)
        {
            EnquiryNo = enquiryno;
            EnquiryDated = enqdated;
            CustomerName = customerName;
            CategoryRef = categoryRef;
            Quantity = quantity;
            TotalReferred = totalReferred;
            TotalSelected = totalSelected;
            TotalRejected = totalRejected;
            TotalUnderClientReview = totalUnderClientReview;
            TotalDeployed = totalDeployed;
            TotalToSendToClient = totalToSendToClient;
            CompleteBy = completeBy;
            Status = status;
        }

        public string CustomerName {get; set; }
        public int EnquiryNo {get; set;}
        public string EnquiryDated {get; set;}
        public string CategoryRef { get; set; }
        public int Quantity { get; set; }
        public int TotalReferred { get; set; }
        public int TotalSelected {get; set;}
        public int TotalRejected {get; set;}
        public int TotalUnderClientReview {get; set;}
        public int TotalDeployed {get; set;}
        public int TotalToSendToClient {get; set;}
        public string CompleteBy { get; set; }
        public string Status {get; set;}
    }
}