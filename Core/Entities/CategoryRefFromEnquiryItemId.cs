using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [NotMapped]
    public class CategoryRefFromEnquiryItemId
    {
        public CategoryRefFromEnquiryItemId(string customerName, string cityName, string categoryRef, string orderNoAndDate)
        {
            CustomerName = customerName;
            CityName = cityName;
            CategoryRef = categoryRef;
            OrderNoAndDate = orderNoAndDate;
        }

        public string CustomerName {get; set;}
        public string CityName {get; set; }
        public string CategoryRef {get; set;}
        public string OrderNoAndDate {get; set;}
        
    }
}