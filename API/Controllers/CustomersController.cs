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
        private readonly ICustomerService _custService;
        private readonly IMapper _mapper;

        public CustomersController(
            IGenericRepository<Customer> custRepo, ICustomerService custService, 
            IMapper mapper
        )
        {
            _mapper = mapper;
            _custRepo = custRepo;
            _custService = custService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<IReadOnlyList<CustomerDto>>>> GetCustomers(
            [FromBody]CustomerSpecParams custParams)
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

//officials

        [HttpGet("officials/{customerId}")]
        public async Task<ActionResult<IReadOnlyList<CustomerOfficialDto>>> GetCustomerOfficials (int customerId)
        {
            var off = await _custService.GetCustomerOfficialList(customerId);
            if (off==null || off.Count ==0) return NotFound(new ApiResponse(404, "No officials on record for selected entity"));
            var offReturn = _mapper.Map<IReadOnlyList<CustomerOfficial>, IReadOnlyList<CustomerOfficialDto>>(off);
            return Ok(offReturn);
        }

        [HttpPost("officials")]
        public async Task<ActionResult<IReadOnlyList<CustomerOfficialDto>>> InsertCustomerOfficials(
            [FromBody]List<CustomerOfficial> officials)
        {
            var offs = await _custService.InsertOfficials(officials);
            if(offs==null || offs.Count==0) return BadRequest(new ApiResponse(404, "Failed to isnert the officials"));
            return Ok(_mapper.Map<IReadOnlyList<CustomerOfficial>, IReadOnlyList<CustomerOfficialDto>>(offs));
        }
    // agents
        [HttpGet("recruitmentAgencies")]
        public async Task<ActionResult<IReadOnlyList<CustomerAgencyNamesDto>>> GetRecruitmentAgencies()
        {
            var cust = await _custService.GetAgencies();
            if (cust == null) return null;

            var dtoList = new List<CustomerAgencyNamesDto>();
            foreach(var item in cust)
            {
                dtoList.Add(new CustomerAgencyNamesDto{
                    Id=item.Id,
                    CustomerName=item.CustomerName,
                    CityName=item.CityName
                });
            }

            return dtoList;
        }
    }
}
