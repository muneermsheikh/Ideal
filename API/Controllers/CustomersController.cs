using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CustomersController : BaseApiController
    {
        private readonly IGenericRepository<Customer> _custRepo;
        private readonly IMapper _mapper;

        public CustomersController(IGenericRepository<Customer> custRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _custRepo = custRepo;
        }

    [HttpGet]
    public async Task<ActionResult<Pagination<CustomerDto>>> GetCategories(
        [FromQuery] CustomerSpecParams custParams)
    {
        var spec = new CustomerSpecs(custParams);
        var countSpec = new CustomerWithFiltersForCountSpec(custParams);
        var totalItems = await _custRepo.CountWithSpecAsync(countSpec);

        var custs = await _custRepo.ListWithSpecAsync(spec);

        var data = _mapper
            .Map<IReadOnlyList<Customer>, IReadOnlyList<CustomerDto>>(custs);
        return Ok(new Pagination<CustomerDto>
            (custParams.PageIndex, custParams.PageSize, totalItems, data));

    }
}
}