using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CustomersController : BaseApiController
    {
        private readonly IGenericRepository<Customer> _custRepo;

        private readonly IMapper _mapper;

        public CustomersController(
            IGenericRepository<Customer> custRepo,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _custRepo = custRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<IReadOnlyList<CustomerDto>>>> GetCustomers(
            [FromQuery]CustomerSpecParams custParams)
        {
            var spec = new CustomerSpecs(custParams);
            var countSpec = new CustomerWithFiltersForCountSpec(custParams);
            var totalItems = await _custRepo.CountWithSpecAsync(countSpec);

            var custs = await _custRepo.ListWithSpecAsync(spec);
            if (custs==null) return BadRequest(new ApiResponse(404));

            var data = _mapper.Map<IReadOnlyList<Customer>, IReadOnlyList<CustomerDto>>(custs);
            return Ok(new Pagination<CustomerDto>(custParams.PageIndex,
                custParams.PageSize, totalItems, data));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> AddCustomer(Customer customer)
        {
            var cust = await _custRepo.AddAsync(customer);
            if (cust == null) return BadRequest(new ApiResponse(400));
            var custToReturn = _mapper.Map<Customer, CustomerDto>(cust);
            return custToReturn;
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerDto>> Updatecustomer(Customer customer)
        {
            var cust = await _custRepo.UpdateAsync(customer);
            if (cust==null) return BadRequest(new ApiResponse(400));

            var custToReturn = _mapper.Map<Customer, CustomerDto>(cust);
            return custToReturn;
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> DeleteCustomer(Customer customer)
        {
            var deleted = await _custRepo.DeleteAsync(customer);
            if (deleted == 0) return BadRequest(new ApiResponse(400, "Failed to delete"));
            return true;
        }
    }
}
