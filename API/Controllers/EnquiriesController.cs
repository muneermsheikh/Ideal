using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    public class EnquiriesController : BaseApiController
    {
        private readonly IEnquiryService _enqService;
        private readonly IMapper _mapper;
        public EnquiriesController(IEnquiryService enqService, IMapper mapper)
        {
            _mapper = mapper;
            _enqService = enqService;
        }

        [HttpPost]
        public async Task<ActionResult<Enquiry>> CreateOrder(EnquiryDto enquiryDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            if (email == null) return BadRequest(new ApiResponse(400, "Client email cannot be retrieved"));
            var address = _mapper.Map<AddressDto, Address>(enquiryDto.ShipToAddress);   
                // 2nd parameter of the Mapping - Address - is from EnquiryAggregate
            var enquiry = await _enqService.CreateEnquiryAsync(email, enquiryDto.BasketId, address);
            
            if (enquiry == null) return BadRequest(new ApiResponse(400, "problem creating Enquiry"));

            return Ok(enquiry);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Enquiry>>> GetEnquiriesOfUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var enquiries = await _enqService.GetUserEnquiriesAsync(email);
            if (enquiries == null) return NotFound(new ApiResponse(400, "No enquiries of logged in User on record"));
            return Ok(enquiries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Enquiry>> GetEnquiryOfUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var enquiry = await _enqService.GetEnquiryById(id, email);
            if (enquiry == null) return NotFound(new ApiResponse(400, "No enquiry belonging to the logged in user with requested Id on record"));
            return Ok(enquiry);
        }

    }
}
