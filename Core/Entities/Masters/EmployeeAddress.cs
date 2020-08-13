namespace Core.Entities.Masters
{
   public class EmployeeAddress: BaseEntity
    {
        public EmployeeAddress()
        {
        }

        public EmployeeAddress(string addressType, string address1, string address2, string city, string pIN, string district, string state)
        {
            AddressType = addressType;
            Address1 = address1;
            Address2 = address2;
            City = city;
            PIN = pIN;
            State = state;
            District = district;
        }

        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public bool Valid { get; set; }  =true;
    }
}