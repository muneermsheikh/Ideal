namespace API.Dtos
{
    public class CustomerAddressDto
    {
        public int Id {get; set;}
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
        public string AddedOn { get; set; }
    }
}