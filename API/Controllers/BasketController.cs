using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        private readonly ICategoryService _catService;
        public BasketController(IBasketRepository basketRepo, IMapper mapper, ICategoryService catService)
        {
            _catService = catService;
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
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            // update categorynames in the dto basket

            var intIds = (from t in basket.Items select t.Id).ToArray();
                
            var cats = await _catService.CategoriesFromCategoryIds(intIds);
            
            foreach(var bskt in basket.Items)
            {
                foreach(var item in cats)                
                {
                    if (bskt.Id == item.Id)
                    {
                        bskt.CategorytName = item.Name;
                        break;
                    }
                }
            }
            //the basket obj now has correct category names
            var custBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updateBasket = await _basketRepo.UpdatebasketAsync(custBasket);
            if (updateBasket == null) return BadRequest(new ApiResponse(400, "Failed to update the basket"));
            return Ok(updateBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string Id)
        {
            await _basketRepo.DeleteBasketAsync(Id);
        }
    }
}