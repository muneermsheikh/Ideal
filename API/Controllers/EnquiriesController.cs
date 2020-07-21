using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Dtos;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
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
            // var address = enquiryDto.ShipToAddress;
            var siteAddress = _mapper.Map<AddressDto, SiteAddress>(enquiryDto.ShipToAddress);
            var CustomerName = HttpContext.User.RetrieveNameFromPrincipal();
            var enquiry = await _enqService.CreateEnquiryAsync(email, enquiryDto.BasketId, 
                siteAddress);

            if (enquiry == null) return BadRequest(new ApiResponse(400, "problem creating Enquiry"));

            return Ok(enquiry);
        }

        [Cached(1000)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<EnquiryToReturnDto>>> GetEnquiriesOfUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var enquiries = await _enqService.GetUserEnquiriesAsync(email);
            if (enquiries == null) return NotFound(new ApiResponse(400, "No enquiries of logged in User on record"));

            return Ok(_mapper.Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryDto>>(enquiries));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnquiryToReturnDto>> GetEnquiryOfUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var enquiry = await _enqService.GetEnquiryById(id, email);
            if (enquiry == null) return NotFound(new ApiResponse(400, "No enquiry belonging to the logged in user with requested Id on record"));
            return Ok(_mapper.Map<Enquiry, Enquiry>(enquiry));
        }

        // Job Description
        [HttpGet("jobdesc")]
        public async Task<ActionResult<JobDesc>> GetJobDescription(int enquiryItemId)
        {
            return await _enqService.GetJobDescriptionBySpecAsync(enquiryItemId);
        }

        [HttpPut("jobdesc")]
        public async Task<ActionResult<JobDesc>> UpdateJobDescription(JobDesc jobDesc)
        {
            return await _enqService.UpdateJDAsync(jobDesc);
        }

        //remuneration
        [HttpGet("remuneration/{enquiryItemId}")]
        public async Task<ActionResult<Remuneration>> GetRemuneration(int enquiryItemId)
        {
            return await _enqService.GetRemunerationBySpecEnquiryItemIdAsync(enquiryItemId);
        }

        [HttpPut("remuneration")]
        public async Task<ActionResult<Remuneration>> UpdateRemuneration(Remuneration remuneration)
        {
            return await _enqService.UpdateRemunerationAsync(remuneration);
        }

        //contract review item
        
        [HttpPut("remuneration")]
        public async Task<ActionResult<int>> UpdateReviewItem(ContractReviewItem reviewItem)
        {
            var remun = await _enqService.UpdateContractReviewItemAsync(reviewItem);
            if (remun == null) return BadRequest(new ApiResponse(404, "Failed to update review item"));

            // POST UPDATE - Update Enquiry.ReadyToReview to true if all enquiry items reviewed 
            int notReviewed = await _enqService.GetEnquiryItemsCountNotReviewed(reviewItem.EnquiryId);
            if (notReviewed > 0) return 0;        // do nothing, as not all items are reviewed

            // all enquiry items reviewed. update Enquiry.ReadyForReview flag
            var enq = await _enqService.GetEnquiryByIdAsync(reviewItem.EnquiryId);
            if (enq == null) return BadRequest(new ApiResponse(404, "Failed to retrieve enquiry by Id"));
            var res = await _enqService.UpdateEnquiryReadyToReview(enq);

            return res ? 1 : 0;

        }


    }
}
