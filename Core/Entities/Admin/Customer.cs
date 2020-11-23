using System;
using System.Collections.Generic;
using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Entities.Admin
{
   public class Customer: BaseEntity
    {
        public Customer()
        {
        }

        public Customer(string customerType, string customerName, string knownAs, 
            string cityName, string email, string mobile, string introducedBy)
        {
            CustomerType = customerType;
            CustomerName = customerName;
            KnownAs = knownAs;
            City = cityName;
            Email = email;
            IntroducedBy = introducedBy;
            Phone1 = mobile;
        }

        public string CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string KnownAs { get; set; }
        public string IntroducedBy { get; set; }
        public string Email {get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string CompanyUrl { get; set; }
        public string Description { get; set; }
        public string Address1 {get; set;}
        public string Address2 {get; set;}
        public string City {get; set;}
        public string Pin {get; set;}
        public string State {get; set;}
        public string Country {get; set;}
        public string CustomerStatus { get; set; } = "active";
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public virtual List<CustomerIndustryType> CustomerIndustryTypes { get; set; }
        public virtual List<CustomerOfficial> CustomerOfficials { get; set; }
        // public virtual Grade Grade { get; set; }     // to be incorported later
        
    }
}