using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketApiController : BaseApiController
    {
        private readonly ICategoryService _catService;
        private readonly IBasketRepository _basketRepo;
        private readonly ICustomerService _custService;
        public BasketApiController(ICategoryService catService, 
            IBasketRepository basketRepo, ICustomerService custService)
        {
            _custService = custService;
            _basketRepo = basketRepo;
            _catService = catService;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasket>> CreateBasket()
        {

            var items = new List<BasketItem>();

            // create test data
            Random random = new Random();

            int Ct = random.Next(2, 4);

            for (int i = 1; i <= Ct; i++)
            {
                int catId = random.Next(1, 10);
                int qnty = random.Next(10);
                var cat = await _catService.CategoryByIdAsync(catId);
                if (cat == null) continue;
                var categoryName = cat.Name;
                qnty = random.Next(1, 20);
                int expDesiredInYrsMin = random.Next(1, 15);
                int expDesiredInYrsMax = random.Next(2, 15);
                var jobDescInBrief = "";
                var sAttach = "";
                var salaryCurrency = "";
                string salaryNegotiable="f";
                int salaryRangeFrom = random.Next(800, 2000);
                int salaryRangeUpto = random.Next(1000, 2500);
                int intAddDays = random.Next(2, 10);

                string provFood;
                int foodAllowance=0;
                string provHousing;
                int housingAllowance=0;
                string provTransport;
                int transportAllowance=0;
                int otherAllowance=0;
                int leavePerYear=15;
                int leaveAfterMonths=24;

                switch (random.Next(0, 2))
                {
                    case 0:
                        provFood = "NotProvided";
                        provHousing = "ProvidedFree";
                        provTransport = "NotProvided";
                        break;
                    default:
                        provFood = "NotProvided";
                        provHousing = "ProvidedFree";
                        provTransport = "NotProvided";
                        break;
                }

                var item = new BasketItem( catId, categoryName, "f", qnty, expDesiredInYrsMin,
                    expDesiredInYrsMax, jobDescInBrief, sAttach, salaryCurrency, salaryNegotiable, 
                    salaryRangeFrom, salaryRangeUpto, 24, leaveAfterMonths, leavePerYear, provFood, 
                    foodAllowance, provHousing, housingAllowance, provTransport, 
                    transportAllowance, otherAllowance, DateTime.Now.AddDays(intAddDays));

                items.Add(item);
            }

            // create the basket
            string newId = Guid.NewGuid().ToString();
            int CustomerId = random.Next(1, 10);
            var offs = await _custService.GetCustomerOfficialList(CustomerId);
            int CustomerOfficialId = offs.Count > 0 ? offs[0].Id : 0;
            var basket = new CustomerBasket(newId, CustomerId, CustomerOfficialId, items);

            var updateBasket = await _basketRepo.UpdatebasketAsync(basket);
            if (updateBasket == null) return BadRequest(new ApiResponse(400, "Failed to update the basket"));

            return Ok(updateBasket);
        }
    }
}