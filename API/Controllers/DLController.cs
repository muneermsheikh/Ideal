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
            var totalItems = await _unitOfWork.Repository<Enquiry>().CountWithSpecAsync(
                new EnquirySpecsCount(eParams) );
            if(totalItems==0) return NotFound(new ApiResponse(400, "No records returned"));
            var enqs = await _unitOfWork.Repository<Enquiry>().ListTop500WithSpecAsync(
                new EnquirySpecs(eParams));
            if (enqs == null) return NotFound(new ApiResponse(400, "your instructions did not find any matching records"));

            var data = _mapper
                .Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryToReturnDto>>(enqs);

            return Ok(new Pagination<EnquiryToReturnDto>
                (eParams.PageIndex, eParams.PageSize, totalItems, data));
        }

        [HttpGet("demandLetter")]
        public async Task<ActionResult<EnquiryToReturnDto>> GetDL(int enquiryId)
        {
            var enq = await _enqService.GetEnquiryByIdAsync(enquiryId);
            if (enq == null) return NotFound(new ApiResponse(400, "There is no record with that Id"));
            var enqItems = enq.EnquiryItems;
            if (enqItems==null) throw new Exception("The Demand Letter has no items");
            foreach (var item in enqItems)
            {
                item.JobDesc = await _enqService.GetJobDescriptionBySpecAsync(item.Id);
                item.Remuneration = await _enqService.GetRemunerationBySpecEnquiryItemIdAsync(item.Id);
                //_mapper.Map<JobDesc, JobDescDto>(item.JobDesc);
                //_mapper.Map<Remuneration, RemunerationDto>(item.Remuneration);
            }

            var retDto = _mapper.Map<Enquiry, EnquiryToReturnDto>(enq);
            return retDto;
        }

        [HttpGet("demandLetterWithSpec")]
        public async Task<ActionResult<IReadOnlyList<EnquiryToReturnDto>>> GetDLs([FromBody] EnquiryParams enqParam)
        {
            var enqs = await _unitOfWork.Repository<Enquiry>().GetEntityListWithSpec(new EnquirySpecs(enqParam));
            if (enqs == null || enqs.Count==0) return NotFound(new ApiResponse(400, "your parameters did not return any record"));

            foreach(var enq in enqs)
            {
                foreach (var item in enq.EnquiryItems)
                {
                    item.JobDesc = await _enqService.GetJobDescriptionBySpecAsync(item.Id);
                    item.Remuneration = await _enqService.GetRemunerationBySpecEnquiryItemIdAsync(item.Id);
                    //_mapper.Map<JobDesc, JobDescDto>(item.JobDesc);
                    //_mapper.Map<Remuneration, RemunerationDto>(item.Remuneration);
                }
            }
            var retDto = _mapper.Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryToReturnDto>>(enqs);
            return Ok(retDto);
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

        [HttpPost("enquiryItems")]
        public async Task<ActionResult<IReadOnlyList<EnquiryItemToReturnDto>>> InsertDLItems(List<EnquiryItem> enquiryItems)
        {
            var enqItem = await _unitOfWork.Repository<EnquiryItem>().AddListAsync(enquiryItems);
            if (enqItem == null) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<IReadOnlyList<EnquiryItem>, IReadOnlyList<EnquiryItemToReturnDto>>(enqItem));
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
        [HttpPost("generatereviewsofdl")]
        public async Task<ActionResult<IReadOnlyList<ReviewItemDto>>> GenerateReviewsOfADL(int enquiryId)
        {
            var rvws = await _dLService.GenerateReviewItemsOfAnEnquiryAsync(enquiryId);
            if (rvws==null || rvws.Count==0) return BadRequest(new ApiResponse(404));
            return Ok(_mapper.Map<IReadOnlyList<ContractReviewItem>, IReadOnlyList<ReviewItemDto>>(rvws));
        }

        [HttpGet("reviewEnquiryItems/{enquiryId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ReviewItemDto>>> GetReviewItemsOfEnquiry(int enquiryId)
        {
            var rvws = await _dLService.GetOrAddReviewItemsOfEnquiryAsync(enquiryId);
            if (rvws == null) return BadRequest(new ApiResponse(400));
            if (rvws.Count == 0) return NotFound(new ApiResponse(404));
            //return Ok(rvws);
            var dto = _mapper.Map<IReadOnlyList<ContractReviewItem>, IReadOnlyList<ReviewItemDto>>(rvws);
            return Ok(dto);
        }   

        [HttpGet("reviewItem/{enquiryItemId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContractReviewItem>> GetReviewItem(int enquiryItemId)
        {
            var rvw = await _dLService.GetOrAddReviewItemAsync(enquiryItemId);
            if (rvw==null) return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<ContractReviewItem, ReviewItemDto>(rvw));
        }

        [HttpPost("reviewItemList")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewItemDto>> AddReviewItemAsync ([FromBody] ContractReviewItem reviewItem)
        {
            var added =  await _unitOfWork.Repository<ContractReviewItem>().AddAsync(reviewItem);
            if (added == null) return BadRequest(new ApiResponse(404, "Bad request, or the review item does not exist"));
            return Ok( _mapper.Map<ContractReviewItem, ReviewItemDto>(added));
        }

        [HttpPut("reviewItemList")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ReviewItemDto>>> UpdateReviewItems(
            [FromBody] List<ContractReviewItem> reviewItems)
        {
            var rvwItems =  await _dLService.UpdateReviewItemListAsync(reviewItems);

            if (rvwItems == 0 ) return BadRequest(new ApiResponse(404, "Bad request, or the review item does not exist"));
            return Ok();
        }

        [HttpPost("reviewitems")]
        public async Task<ActionResult<IReadOnlyList<ReviewItemDto>>> InsertReviewItems(IReadOnlyList<ContractReviewItem> reviewItems)
        {
            var rvwItems = await _unitOfWork.Repository<ContractReviewItem>().AddListAsync(reviewItems);
            if (rvwItems == null || rvwItems.Count==0) return BadRequest(new ApiResponse(400));
            return Ok( _mapper.Map<IReadOnlyList<ContractReviewItem>, 
                IReadOnlyList<ReviewItemDto>>(rvwItems));
        }


// forward requirement to HR Department

        [HttpPost("enqforwardtoHR")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DLForwardedToHRDto>> SendEnquiryToHR(EnquiryToForwardToHRDto enqToFwd)
        {
            var dlFwd = await _dlForwardService.DLForwardToHRAsync(
                enqToFwd.dtForwarded, enqToFwd.enquiryIds, enqToFwd.EmployeeId);

            if (dlFwd == null) return BadRequest(new ApiResponse(400));
            
            return _mapper.Map<DLForwarded, DLForwardedToHRDto>(dlFwd);
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
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<EnquiryForwarded>>> ForwardToAssociates(
            DLToForwardToAssociatesDto enqToFwd)
        {
            var dlFwd = await _dlForwardService.DLForwardToAssociatesAsync(
                enqToFwd.OfficialIds, enqToFwd.EnqId, enqToFwd.EnqItemIds,
                enqToFwd.mode, enqToFwd.DateForwarded);
            if (dlFwd == null) return BadRequest(new ApiResponse(400, "failed to forward the Demand letters to Associates"));
            
            return Ok(dlFwd);
            //return Ok( _mapper.Map<IReadOnlyList<EnquiryForwarded>,
                //IReadOnlyList<EnqForwardedToAssociatesDto>>(dlFwd));
        }


        [HttpGet("enqforwardlist")]
        public async Task<ActionResult<Pagination<IReadOnlyList<EnquiryForwardedDto>>>> GetEnquiriesForwardedList(
            [FromQuery]EnquiryForwardedParams enqParams)
        {
            var _enqFwdRepo = _unitOfWork.Repository<EnquiryForwarded>();

            var spec = new EnquiryForwardedSpecs(enqParams);
            var countSpec = new EnquiryForwardedForCountSpecs(enqParams);
            var totalItems = await _enqFwdRepo.CountWithSpecAsync(countSpec);

            var enqFwds = await _enqFwdRepo.ListWithSpecAsync(spec);
            if (enqFwds == null || enqFwds.Count == 0) return NotFound(new ApiResponse(404));

            var data = _mapper.Map<IReadOnlyList<EnquiryForwarded>, 
                IReadOnlyList<EnquiryForwardedDto>>(enqFwds);

            return Ok(new Pagination<EnquiryForwardedDto>
                (enqParams.PageIndex, enqParams.PageSize, totalItems, data));
        }

        [HttpGet("enqforward/{enquiryId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<EnquiryForwardedDto>>> GetEnquiryForwardedOfEnquiry(
            int enquiryId)
        {
            var _enqFwdRepo = _unitOfWork.Repository<EnquiryForwarded>();
            //var totalitems = await _enqFwdRepo.CountWithSpecAsync(new EnquiryForwardedForCountSpecs(enquiryId));
            var enqFwd = await _enqFwdRepo.GetEntityListWithSpec(new EnquiryForwardedSpecs(enquiryId));
            if (enqFwd == null) return NotFound(new ApiResponse(404));

            return Ok( _mapper.Map<IReadOnlyList<EnquiryForwarded>, IReadOnlyList<EnquiryForwardedDto>>(enqFwd));
        }


    }
}