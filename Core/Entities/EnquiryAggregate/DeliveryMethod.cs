namespace Core.Entities.EnquiryAggregate
{
    public class DeliveryMethod: BaseEntity
    {
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}