using System;
using System.Collections.Generic;
using System.Linq;
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
        // this method is called by Agency Officials, here basket is not relevant
        [HttpPost]
        public async Task<ActionResult<Enquiry>> AddDL(Enquiry enq)
        {
            var enqAdded = await _enqService.AddEnquiryAsync(enq);
            if (enqAdded == null) return BadRequest(new ApiResponse(400));
            return Ok(enqAdded);
        }
        
        [HttpPut]
        public async Task<ActionResult<Enquiry>> UpdateDL (Enquiry enq)
        {
            var enqUpdated = await _enqService.UpdateDLAsync(enq);
            if (enqUpdated == null) return BadRequest(new ApiResponse(400));
            return Ok(enqUpdated);
        }
        [HttpDelete]
        public async Task<ActionResult<int>> DeleteDL(int id)
        {
            var deleted = await _enqService.DeleteEnquiryAsync(id);
            if (deleted == 0) return BadRequest(new ApiResponse(400));
            return Ok(deleted);
        }
        
        [HttpGet("dlindex")]
        public async Task<ActionResult<Pagination<Enquiry>>> DLIndex(
            [FromQuery] EnquiryParams eParams)
        {
            var totalItems = await _unitOfWork.Repository<Enquiry>().CountWithSpecAsync(
                new EnquirySpecsCount(eParams) );
            if(totalItems==0) return NotFound(new ApiResponse(400, "No records returned"));
            var enqs = await _unitOfWork.Repository<Enquiry>().ListTop500WithSpecAsync(
                new EnquirySpecs(eParams));
            if (enqs == null) return NotFound(new ApiResponse(400, "your instructions did not find any matching records"));

            //var data = _mapper
            //    .Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryToReturnDto>>(enqs);

            return Ok(new Pagination<Enquiry>
                (eParams.PageIndex, eParams.PageSize, totalItems, enqs));
        }

        [HttpGet("demandLetter")]
        public async Task<ActionResult<Enquiry>> GetDL(int enquiryId)
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

            // var retDto = _mapper.Map<Enquiry, EnquiryToReturnDto>(enq);
            return enq;
        }

        [HttpGet("demandLetterWithSpec")]
        public async Task<ActionResult<IReadOnlyList<Enquiry>>> GetDLs([FromBody] EnquiryParams enqParam)
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
            // var retDto = _mapper.Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryToReturnDto>>(enqs);
            return Ok(enqs);
        }

        [HttpGet("listWithStatus")]
        public async Task<ActionResult<Pagination<List<EnquiryWithAllStatusDto>>>> DLIndexWithStatus ([FromQuery] EnquiryParams eParams)
        {
         /*   var enqs = await _dLService.GetEnquiryList500WithAllStatus();
            var enqDto = _mapper.Map<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryWithAllStatusDto>>(enqs);
            // var enqDto = MapEnquiryToEnquiryListWithStatus(enqs.ToList());
            return enqDto;
        */
            var totalItems = await _unitOfWork.Repository<Enquiry>().CountWithSpecAsync(
                new EnquirySpecsCount(eParams) );
            if(totalItems==0) return NotFound(new ApiResponse(404, "No records returned"));
            var enqs = await _unitOfWork.Repository<Enquiry>().ListTop500WithSpecAsync(
                new EnquirySpecs(eParams));
            if (enqs == null || enqs.Count == 0) return NotFound(new ApiResponse(400, "your instructions did not find any matching records"));
            // var enqDto = MapEnquiryToEnquiryListWithStatus(enqs.ToList());
            var enqDto = _mapper.Map<List<Enquiry>, List<EnquiryWithAllStatusDto>>(enqs.ToList());
            return Ok(enqDto);

        }

        private List<EnquiryWithAllStatusDto> MapEnquiryToEnquiryListWithStatus(List<Enquiry> enquiries)
        {
            
            if (enquiries == null || enquiries.Count == 0)
            {
                return null;
            }

            List<EnquiryWithAllStatusDto> enqList = new List<EnquiryWithAllStatusDto>();
            
            foreach(var enq in enquiries)
            {
                int qntySum = 0;
                bool NotAssigned=false;
                
                foreach(var item in enq.EnquiryItems)
                {
                    qntySum += item.Quantity;
                    if (item.TasksAssigned != null)
                    {
                        foreach(var tsk in item.TasksAssigned)
                        {
                            if (NotAssigned) {break;}
                            if (!(tsk.TaskType == "HRExecutive" && (
                                "taskstartedtaskcompletedtaskcanceledtaskclosed".Contains(tsk.TaskStatus.ToLower())))) 
                            {
                                NotAssigned = true;
                                break;
                            }
                        }
                    }
                }
            
                string TaskAssigned = NotAssigned==false ? "Not Assigned" : "Assigned";
        
                var newDto = new EnquiryWithAllStatusDto
                (
                    enq.Id, enq.CustomerId, "not defined", "new city", "saudi arabia", enq.EnquiryNo,
                    enq.EnquiryDate, enq.EnquiryRef ?? "not defined", enq.BasketId ?? "undefined", 
                    enq.EnquiryItems.Count, qntySum, DateTime.Today, "proj manager", TaskAssigned, enq.ReviewStatus ?? "not defined",
                    enq.EnquiryStatus ?? "not defined"
                );
                enqList.Add(newDto);
            }

            return enqList;
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
        public async Task<ActionResult<EnquiryItem>> GetDLItem(int enquiryItemId)
        {
            
            var items = await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(
                new EnquiryItemsSpecs(enquiryItemId));
            
            return Ok(items);
        }

        [HttpGet("enquiryItemEdit")]
        public async Task<ActionResult<EnquiryItem>> GetDLItemToEdit(int enquiryItemId)
        {
            
            return Ok(await _unitOfWork.Repository<EnquiryItem>().GetEntityWithSpec(
                new EnquiryItemsSpecs(enquiryItemId)));
        }

        [HttpPut("enquiryItem")]
        public async Task<ActionResult<EnquiryItem>> UpdateDLItem(EnquiryItem enquiryItem)
        {
            // var item = _mapper.Map<EnquiryItemToEditDto, EnquiryItem>(enquiryItem);
            var editedItem = await _dLService.UpdateDLItemAsync(enquiryItem);
            if (editedItem == null) return BadRequest(new ApiResponse(400));
            return Ok(editedItem);
        }

        [HttpDelete("enquiryItem")]
        public async Task<ActionResult<bool>> DeleteDLItem(EnquiryItem enquiryItem)
        {
            var deleted = await _unitOfWork.Repository<EnquiryItem>().DeleteAsync(enquiryItem);
            if (deleted == 1) return Ok();
            return BadRequest(new ApiResponse(400));
        }

        [HttpPost("enquiryItems")]
        public async Task<ActionResult<IReadOnlyList<EnquiryItem>>> InsertDLItems(List<EnquiryItem> enquiryItems)
        {
            var enqItem = await _unitOfWork.Repository<EnquiryItem>().AddListAsync(enquiryItems);
            if (enqItem == null) return BadRequest(new ApiResponse(400));
            return Ok(enqItem);
        }
         

// Job Description
        [HttpGet("jd/{enquiryItemId}")]
        public async Task<ActionResult<JobDesc>> GetJobDescription(int enquiryItemId)
        {
            return await _enqService.GetJobDescriptionBySpecAsync(enquiryItemId);
        }

        [HttpPut("jd")]
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
        public async Task<ActionResult<IReadOnlyList<ContractReviewItem>>> GenerateReviewsOfADL(int enquiryId)
        {
            var rvws = await _dLService.GenerateReviewItemsOfAnEnquiryAsync(enquiryId);
            if (rvws==null || rvws.Count==0) return BadRequest(new ApiResponse(404));
            return Ok(rvws);
        }

        [HttpGet("reviewEnquiryItems/{enquiryId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ContractReviewItem>>> GetReviewItemsOfEnquiry(int enquiryId)
        {
            var rvws = await _dLService.GetOrAddReviewItemsOfEnquiryAsync(enquiryId);
            if (rvws == null) return BadRequest(new ApiResponse(400));
            if (rvws.Count == 0) return NotFound(new ApiResponse(404));
            //return Ok(rvws);
            return Ok(rvws);
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
        public async Task<ActionResult<ContractReviewItem>> AddReviewItemAsync ([FromBody] ContractReviewItem reviewItem)
        {
            var added =  await _unitOfWork.Repository<ContractReviewItem>().AddAsync(reviewItem);
            if (added == null) return BadRequest(new ApiResponse(404, "Bad request, or the review item does not exist"));
            return Ok(added);
        }

        [HttpPut("reviewItemList")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ContractReviewItem>>> UpdateReviewItems(
            [FromBody] List<ContractReviewItem> reviewItems)
        {
            var rvwItems =  await _dLService.UpdateReviewItemListAsync(reviewItems);

            if (rvwItems == 0 ) return BadRequest(new ApiResponse(404, "Bad request, or the review item does not exist"));
            return Ok(reviewItems);
        }

        [HttpPost("reviewitems")]
        public async Task<ActionResult<IReadOnlyList<ContractReviewItem>>> InsertReviewItems(IReadOnlyList<ContractReviewItem> reviewItems)
        {
            var rvwItems = await _unitOfWork.Repository<ContractReviewItem>().AddListAsync(reviewItems);
            if (rvwItems == null || rvwItems.Count==0) return BadRequest(new ApiResponse(400));
            return Ok(rvwItems);
        }


// forward requirement to HR Department


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

            return Ok(_mapper.Map<IReadOnlyList<EnquiryForwarded>, IReadOnlyList<EnquiryForwardedDto>>(enqFwd));
        }


    }
}