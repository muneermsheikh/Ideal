using Core.Entities.EnquiryAggregate;

namespace API.Dtos
{
    public class EnquiryDto
    {
        public string BasketId { get; set; }
        public int CustomerId {get; set;}
        public int OfficialId {get; set; }
        //public SiteAddress ShipToAddress { get; set; }
    }
}