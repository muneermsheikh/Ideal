using System.Threading.Tasks;
using Core.Entities.apiBasket;

namespace Core.Interfaces
{
    public interface IBasketService
    {
         public Task<apiBasket> CreateBasket();
         public Task<apiBasket> DeleteBasket();
         public Task<apiBasket> UpdateBasket(apiBasket basket);
         public string GetGUID();

    }
}