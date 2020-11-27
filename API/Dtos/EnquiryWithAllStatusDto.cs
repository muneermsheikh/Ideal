using System;

namespace API.Dtos
{
    public class EnquiryWithAllStatusDto
    {

        public EnquiryWithAllStatusDto() {}
        
        public EnquiryWithAllStatusDto(int id, int customerId, string customerName, string city, string country, int enquiryNo, 
            DateTime enquiryDate, string enquiryRef, string basketId, int noOfCategories, int sumOfQuantities, 
            DateTime completeBy, string projectManager, string assignedToHRExecutives, string reviewStatus, string enquiryStatus) 
        {
            this.Id = id;
                this.CustomerId = customerId;
                this.CustomerName = customerName;
                this.City = city;
                this.Country = country;
                this.EnquiryNo = enquiryNo;
                this.EnquiryDate = enquiryDate;
                this.EnquiryRef = enquiryRef;
                this.BasketId = basketId;
                this.NoOfCategories = noOfCategories;
                this.SumOfQuantities = sumOfQuantities;
                this.CompleteBy = completeBy;
                this.ProjectManager = projectManager;
                this.AssignedToHRExecutives = assignedToHRExecutives;
                this.ReviewStatus = reviewStatus;
                this.EnquiryStatus = enquiryStatus;
               
        }
                public int Id {get; set; }
        public int CustomerId {get; set; }
        public string CustomerName {get; set;}
        public string City {get; set;}
        public string Country {get; set; }
        public int EnquiryNo { get; set; }
        public DateTime EnquiryDate { get; set; }
        
        public string EnquiryRef { get; set; }
        public string BasketId {get; set;}
        public int NoOfCategories {get; set;}
        public int SumOfQuantities {get; set;}
        public DateTime CompleteBy { get; set; }
        public string ProjectManager {get; set;}
        public string AssignedToHRExecutives {get; set;}
        public string ReviewStatus {get; set; }
        public string EnquiryStatus { get; set; }
        
    }
}