using System;
using System.Threading.Tasks;
using Core.Entities.apiBasket;
using Core.Interfaces;


namespace Infrastructure.Services
{
    public class BasketService : IBasketService
    {
        public Task<apiBasket> CreateBasket()
        {
            throw new System.NotImplementedException();
        }

        public Task<apiBasket> DeleteBasket()
        {
            throw new System.NotImplementedException();
        }

        public string GetGUID()
        {
            return Guid.NewGuid().ToString();   
        }

        public Task<apiBasket> UpdateBasket(apiBasket basket)
        {
            throw new System.NotImplementedException();
        }

        
    }
}