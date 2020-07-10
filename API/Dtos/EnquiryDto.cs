namespace API.Dtos
{
    public class EnquiryDto
    {
        public string BasketId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}