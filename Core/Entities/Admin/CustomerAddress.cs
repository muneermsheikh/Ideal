using System;

namespace Core.Entities.Admin
{
   public class CustomerAddress: BaseEntity
    {
        public CustomerAddress(int customerId, string addressType, string address1, string address2, 
            string city, string pIN, string state, string district, string country)
        {
            CustomerId = customerId;
            AddressType = addressType;
            Address1 = address1;
            Address2 = address2;
            City = city;
            PIN = pIN;
            State = state;
            District = district;
            Country = country;
        }
        public CustomerAddress(string addressType, string address1, string address2, 
            string city, string pIN, string state, string district, string country)
        {
            AddressType = addressType;
            Address1 = address1;
            Address2 = address2;
            City = city;
            PIN = pIN;
            State = state;
            District = district;
            Country = country;
        }

        public CustomerAddress()
        {
        }

        public int CustomerId { get; set; }
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public bool Valid { get; set; } = true;
        public DateTime AddedOn { get; set; } = DateTime.Now.Date;
    }
}