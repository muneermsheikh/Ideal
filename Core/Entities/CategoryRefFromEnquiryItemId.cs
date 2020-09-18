using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [NotMapped]
    public class CategoryRefFromEnquiryItemId
    {
        public CategoryRefFromEnquiryItemId(string customerName, string cityName, int enquiryid, string categoryRef, string orderNoAndDate)
        {
            EnquiryId=enquiryid;
            CustomerName = customerName;
            CityName = cityName;
            CategoryRef = categoryRef;
            OrderNoAndDate = orderNoAndDate;
        }

        public int EnquiryId {get; set;}
        public string CustomerName {get; set;}
        public string CityName {get; set; }
        public string CategoryRef {get; set;}
        public string OrderNoAndDate {get; set;}
        
    }
}