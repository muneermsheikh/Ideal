using System.Collections.Generic;

namespace Core.Entities.EnquiryAggregate
{
   public class CustomerBasket
    {
        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }

        public CustomerBasket(string id, int customerId, int officialId, List<BasketItem> basketItems)
        {
            Id = id;
            Items = basketItems;
            CustomerId = customerId;
            OfficialId = officialId;
        }

        public string Id { get; set; }
        public int CustomerId {get; set; }
        public int OfficialId {get; set;}
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public string ClientSecret { get; set; }
    }
}