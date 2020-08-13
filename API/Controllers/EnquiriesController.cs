using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnquiryToReturnDto>> CreateOrder(EnquiryDto enquiryDto)
        {
            var email= HttpContext.User.RetrieveEmailFromPrincipal();
            if (email == null) return BadRequest(new ApiResponse(400, "Client email cannot be retrieved"));
        
            //var address = enquiryDto.ShipToAddress;

            var enquiry = await _enqService.CreateEnquiryAsync(enquiryDto.BasketId);

            if (enquiry == null) return BadRequest(new ApiResponse(400, "problem creating Enquiry"));

            // *** create acknowledgement to client by email / WhatsApp / SMS
            var enqToReturn = _mapper.Map<Enquiry, EnquiryToReturnDto>(enquiry);
            
            //add customer object
            enqToReturn.Customer = _mapper.Map<Customer, CustomerToReturnDto>(
                await _enqService.CustomerToReturn(enqToReturn.CustomerId));
            
            //add customer official HR Executive object
            /* if ((int)enquiry.HRExecutiveId !=0) 
            {enqToReturn.CustOfficialHRExec = _mapper.Map<CustomerOfficial, CustOfficialToReturnDto>(
                await _enqService.CustomerOfficialToReturn((int)enquiry.HRExecutiveId));
            }
            */ 
            
            //add enquiry items
            enqToReturn.enquiryItems = _mapper.Map<IReadOnlyList<EnquiryItem>,
                IReadOnlyList<EnquiryItemToReturnDto>>(enquiry.EnquiryItems);
            return enqToReturn;
        }

        [Cached(1000)]
        [HttpGet("customerId")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<EnquiryToReturnDto>>> GetEnquiriesOfUser(int customerId)
        {
            var specs = new EnquirySpecs(customerId, "");       // second paramter is blank dummy, bcz there is another sub with one int parameter
            var enquiries = await _enqService.GetUserEnquiriesAsync(customerId);
            if (enquiries == null) return NotFound(new ApiResponse(404, "No enquiries of the selected User on record"));

            return Ok(_mapper.Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryDto>>(enquiries));
        }

    }
}
