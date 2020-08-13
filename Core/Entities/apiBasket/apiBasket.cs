using System.Collections.Generic;

namespace Core.Entities.apiBasket
{
    public class apiBasket
    {
        public string Id {get; set; }
        public List<apiBasketItem> apiBasketItems {get; set; }
    }
}