using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository basketRepo, IMapper mapper)
        {
            _mapper = mapper;
            _basketRepo = basketRepo;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string Id)
        {
            var basket = await _basketRepo.GetBasketAsync(Id);
            return Ok(basket ?? new CustomerBasket(Id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var custBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updateBasket = await _basketRepo.UpdatebasketAsync(custBasket);
            return Ok(updateBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string Id)
        {
            await _basketRepo.DeleteBasketAsync(Id);
        }
    }
}