using System;

namespace Core.Entities.Admin
{
   public class CustomerAddress: BaseEntity
    {
        public int CustomerId { get; set; }
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public bool Valid { get; set; }
        public DateTime AddedOn { get; set; }
    }
}