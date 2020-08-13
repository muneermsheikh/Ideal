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

        /*
        public Customer(enumCustomerType customerType, string customerName, string knownAs, 
            string cityName, string email, string mobile, string introducedBy, CustomerAddress  custAddress)
        {
            CustomerType = customerType;
            CustomerName = customerName;
            KnownAs = knownAs;
            CityName = cityName;
            Email = email;
            IntroducedBy = introducedBy;
            Mobile = mobile;
            CustomerAddress = custAddress;
        }
        */ 
        
        public Customer(enumCustomerType customerType, string customerName, string knownAs, 
            string cityName, string email, string mobile, string introducedBy)
        {
            CustomerType = customerType;
            CustomerName = customerName;
            KnownAs = knownAs;
            CityName = cityName;
            Email = email;
            IntroducedBy = introducedBy;
            Mobile = mobile;
        }

        public enumCustomerType CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string KnownAs { get; set; }
        public string CityName { get; set; }
        public virtual List<IndustryType> IndustryTypes { get; set; }
        public string IntroducedBy { get; set; }
        public string Email {get; set; }
        public string Mobile { get; set; }
        public string Phone2 { get; set; }
        public string CompanyUrl { get; set; }
        public string Description { get; set; }
        public enumCustomerStatus CustomerStatus { get; set; } = enumCustomerStatus.Active;

        public virtual CustomerAddress CustomerAddress {get; set; }
        public int? CustomerAddressId { get; set; }
        public virtual List<CustomerOfficial> CustomerOfficials { get; set; }
        public int? CustomerOfficialId { get; set; }
        public DateTime AddedOn { get; set; } = DateTime.Now;
        public virtual Grade Grade { get; set; }
        
    }
}