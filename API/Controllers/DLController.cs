using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// This controller contains all aspects of the Enquiry object after it is contract reviewed and
// approved - i.e. it becomes an Order (called Demand Letter in overseas recruitment terms).
// this controller is accessed only by company staff.
namespace API.Controllers
{
    public class DLController : BaseApiController
    {
        //private readonly IGenericRepository<EnquiryForwarded> _enqFwdRepo;
        private readonly IEnquiryService _enqService;
        private readonly IDLForwardService _dlForwardService;
        private readonly IMapper _mapper;
        //private readonly IGenericRepository<Enquiry> _enqRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDLService _dLService;

        public DLController(IEnquiryService enqService,
            //IGenericRepository<EnquiryForwarded> enqFwdRepo,
            //IGenericRepository<Enquiry> enqRepo, 
            IUnitOfWork unitOfWork,
            IDLService dLService,
            IDLForwardService dlForwardService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dLService = dLService;
            //_enqRepo = enqRepo;
            _dlForwardService = dlForwardService;
            _mapper = mapper;
            _enqService = enqService;
            //_enqFwdRepo = enqFwdRepo;
        }

//DL
        [HttpGet("dlindex")]
        public async Task<ActionResult<Pagination<EnquiryToReturnDto>>> DLIndex(
            [FromQuery] EnquiryParams eParams)
        {
            var specs = new EnquirySpecs(eParams);
            var specsCt = new EnquirySpecsCount(eParams);
            //var totalItems = await _enqRepo.CountWithSpecAsync(specsCt);
            var totalItems = await _unitOfWork.Repository<Enquiry>().CountWithSpecAsync(specsCt);

            //var enqs = await _enqRepo.ListTop500WithSpecAsync(specs);
            var enqs = await _unitOfWork.Repository<Enquiry>().ListTop500WithSpecAsync(specs);
            if (enqs == null) return NotFound(new ApiResponse(404, "your instructions did not find any matching records"));

            var data = _mapper
                .Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryToReturnDto>>(enqs);

            return Ok(new Pagination<EnquiryToReturnDto>
                (eParams.PageIndex, eParams.PageSize, totalItems, data));
        }

        [HttpGet("demandLetter")]
        public async Task<ActionResult<EnquiryToReturnDto>> GetDL(int enquiryId)
        {
            var enq = await _enqService.GetEnquiryWithSpecByIdAsync(enquiryId);
            if (enq == null) return BadRequest(new ApiResponse(400));

            foreach (var item in enq.EnquiryItems)
            {
                item.JobDesc = await _enqService.GetJobDescriptionBySpecAsync(item.Id);
                item.Remuneration = await _enqService.GetRemunerationBySpecEnquiryItemIdAsync(item.Id);
                //_mapper.Map<JobDesc, JobDescDto>(item.JobDesc);
                //_mapper.Map<Remuneration, RemunerationDto>(item.Remuneration);
            }

            var retDto = _mapper.Map<Enquiry, EnquiryToReturnDto>(enq);
            return retDto;
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteDL(Enquiry enquiry)
        {
            var deleted = await _unitOfWork.Repository<Enquiry>().DeleteAsync(enquiry);
            if (deleted == 1) return Ok();
            return BadRequest(new ApiResponse(400));
        }

//enquiry item
        [HttpGet("enquiryItem")]
        public async Task<ActionResult<EnquiryItemToReturnDto>> GetDLItem(int enquiryItemId)
        {
            return Ok( _mapper.Map<EnquiryItem, EnquiryItemToReturnDto>(
                await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(
                new EnquiryItemsSpecs(enquiryItemId))));
        }

        [HttpGet("enquiryItemEdit")]
        public async Task<ActionResult<EnquiryItemToEditDto>> GetDLItemToEdit(int enquiryItemId)
        {
            return Ok( _mapper.Map<EnquiryItem, EnquiryItemToEditDto>(
                await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(
                new EnquiryItemsSpecs(enquiryItemId))));
        }

        [HttpPut("enquiryItem")]
        public async Task<ActionResult<EnquiryItemToEditDto>> UpdateDLItem(EnquiryItemToEditDto enquiryItem)
        {
            var item = _mapper.Map<EnquiryItemToEditDto, EnquiryItem>(enquiryItem);
            var editedItem = await _dLService.UpdateDLItemAsync(item);
            if (editedItem == null) return BadRequest(new ApiResponse(400));
            return _mapper.Map<EnquiryItem, EnquiryItemToEditDto>(editedItem);
        }

        [HttpDelete("enquiryItem")]
        public async Task<ActionResult<bool>> DeleteDLItem(EnquiryItem enquiryItem)
        {
            var deleted = await _unitOfWork.Repository<EnquiryItem>().DeleteAsync(enquiryItem);
            if (deleted == 1) return Ok();
            return BadRequest(new ApiResponse(400));
        }

        [HttpPost("enquiryItem")]
        public async Task<ActionResult<EnquiryItemToReturnDto>> InsertDLItem(EnquiryItem enquiryItem)
        {
            var enqItem = await _unitOfWork.Repository<EnquiryItem>().AddAsync(enquiryItem);
            if (enqItem == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<EnquiryItem, EnquiryItemToReturnDto>(enqItem));
        }

// Job Description
        [HttpGet("jobdesc/{enquiryItemId}")]
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
        [HttpGet("reviewEnquiryItems/{enquiryId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ReviewItemDto>>> GetReviewItemsOfEnquiry(int enquiryId)
        {
            var rvws = await _dLService.GetOrAddReviewItemsOfEnquiryAsync(enquiryId);
            if (rvws == null) return BadRequest(new ApiResponse(400));
            if (rvws.Count == 0) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<IReadOnlyList<ContractReviewItem>, IReadOnlyList<ReviewItemDto>>(rvws));
        }

        [HttpGet("reviewItem/{enquiryItemId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContractReviewItem>> GetReviewItem(int enquiryItemId)
        {
            var rvw = await _dLService.GetOrAddReviewItemAsync(enquiryItemId);
            if (rvw==null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<ContractReviewItem, ReviewItemDto>(rvw));
        }

        [HttpPut("reviewItemList")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<int> UpdateReviewItem(List<ContractReviewItem> reviewItems)
        {
            return  await _dLService.UpdateReviewItemListAsync(reviewItems);
        /*
            var reviews = await _dLService.UpdateReviewItemsAsync(reviewItems);
            if (reviews == null) return BadRequest(new ApiResponse(400, "Failed to update review item"));

            // POST UPDATE - Update Enquiry.ReadyToReview to true if all enquiry items reviewed 
            int notReviewed = await _enqService.GetEnquiryItemsCountNotReviewed(reviewItems[0].EnquiryId);
            if (notReviewed > 0) return Ok(reviews);        // do nothing, as not all items are reviewed

            // all enquiry items reviewed. update Enquiry.ReadyForReview flag
            var enq = await _enqService.GetEnquiryWithSpecByIdAsync(reviewItems[0].EnquiryId);
            if (enq == null) return BadRequest(new ApiResponse(400, "Failed to retrieve enquiry by Id"));
            var res = await _enqService.UpdateEnquiryReadyToReview(enq);

            return Ok(reviews);
        */
        }


// forward requirement to HR Department

        [HttpPost("enqforwardtoHR")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DLForwarded>> SendEnquiryToHR(EnquiryToForwardToHRDto enqToFwd)
        {
            var dlFwd = await _dlForwardService.DLForwardToHRAsync(
                enqToFwd.dtForwarded, enqToFwd.enquiryIds, enqToFwd.EmployeeId);

            if (dlFwd == null) return BadRequest(new ApiResponse(400));
            
            return dlFwd;
            /*
            dlFwd.AssignedTo = dlFwd.AssignedTo;
           // dlFwd.AssignedToName="new name";
            dlFwd.AssignedOn=dlFwd.AssignedOn;
            var dlForwarded = new DLForwardedToHRDto();
            dlForwarded.Enquiries = _mapper.Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryInBriefDto>>(dlFwd.Enquiries);
            dlForwarded.AssignedOn = enqToFwd.dtForwarded;
            dlForwarded.AssignedToName = "";
            return Ok(dlForwarded);
            */
        }

//forward to associates
        [HttpPost("enqForwardToAssociates")]
        public async Task<ActionResult<IReadOnlyList<EnquiryForwarded>>> ForwardToAssociates(
            DLToForwardToAssociatesDto enqToFwd)
        {
            var dlFwd = await _dlForwardService.DLForwardToAssociatesAsync(
                enqToFwd.OfficialIds, enqToFwd.EnqId, enqToFwd.EnqItemIds,
                enqToFwd.mode, enqToFwd.DateForwarded);
            if (dlFwd == null) return BadRequest(new ApiResponse(400, "failed to forward the Demand letters to Associates"));
            return Ok(dlFwd);
        }


        [HttpGet("enqforwardlist")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<EnquiryForwarded>>> GetEnquiriesForwardedList
            ([FromQuery] EnqForwardSpecParams enqParams)
        {
            var _enqFwdRepo = _unitOfWork.Repository<EnquiryForwarded>();

            var spec = new EnqForwardedWithFilterSpec(enqParams);
            var countSpec = new EnqForwardedWithFiltersForCountSpec(enqParams);
            var totalItems = await _enqFwdRepo.CountWithSpecAsync(countSpec);

            var enqFwds = await _enqFwdRepo.ListWithSpecAsync(spec);
            if (enqFwds == null) return NotFound(new ApiResponse(400));

            // var data = _mapper
            // .Map<IReadOnlyList<EnquiryForwarded>, IReadOnlyList<API.Dtos.EnqForwardedDto>>(enqFwds);
            return Ok(new Pagination<EnquiryForwarded>
                (enqParams.PageIndex, enqParams.PageSize, totalItems, enqFwds));
        }

    }
}