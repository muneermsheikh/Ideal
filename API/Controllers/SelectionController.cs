using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Identity;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Entities.Dto;

namespace API.Controllers
{
    //[Authorize]
    public class SelectionController : BaseApiController
    {
        private readonly ISelDecisionService _selService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SelectionController(ISelDecisionService selService, UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _selService = selService;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<IReadOnlyList<SelDecision>>> InsertSelectionDecisions(SelDecisionToAddDto selDecDto)
        {
            var sel = await _selService.InsertSelDecision(selDecDto);
            if (sel==null) return BadRequest(new ApiResponse(404, "failed to register the selections" ));

            return Ok(sel);

        }


        [HttpGet("pending")]
        public async Task<ActionResult<Pagination<IReadOnlyList<CVRefDto>>>> PendingSelections(CVRefParam cvrefParam)
        {
            var spec = new CVRefSpecs();
            var specCt = new CVRefSpecsWithCount();
            var totalItems = await _unitOfWork.Repository<CVRef>().CountWithSpecAsync(specCt);
            if (totalItems==0) return NotFound(new ApiResponse(400, "no cvs are pending selection as on date"));

            var sel = await _unitOfWork.Repository<CVRef>().GetEntityListWithSpec(spec);
            if (sel==null || sel.Count==0) throw new Exception ("logic error - count with specs is " + totalItems + " but GetEntityListWithSpec returns 0");
            
            var data = _mapper.Map<IReadOnlyList<CVRef>, IReadOnlyList<CVRefDto>>(sel);

            return Ok(new Pagination<CVRefDto>(cvrefParam.PageIndex, cvrefParam.PageSize, totalItems, data));

        }

        [HttpGet("pendingofdl")]
        public async Task<ActionResult<IReadOnlyList<CVRef>>> PendingSelectionsOfADL(int[] enquiryIds)
        {
            var sel = await _selService.GetPendingSelectionsOfDLs(enquiryIds);
            if (sel==null || sel.Count==0) return NotFound(new ApiResponse(400, "No pending selections for the selected DL"));
            return Ok(sel);
        }
        

        [HttpGet("pendingofitem")]
        public async Task<ActionResult<IReadOnlyList<CVRef>>> PendingSelectionsOfDLItem(int[] enquiryItemIds)
        {
            var sel = await _selService.GetPendingSelectionsOfDLItems(enquiryItemIds);
            if (sel==null || sel.Count==0) return NotFound(new ApiResponse(400, "No pending selections for the selected DL Category"));
            return Ok(sel);
        }
        

        [HttpGet]
        public async Task<ActionResult<SelDecision>> UpdateSelDecision(SelDecision selDecision)
        {
            var sel = await _selService.UpdateSelDecision(selDecision);
            if (sel==null) return BadRequest(new ApiResponse(404, "Failed to update the selection object"));
            return sel;
        }

        [HttpDelete]
        public async Task<bool> DeleteSelDecision(SelDecision selDecision)
        {
            var b = await _selService.DeleteSelDecision(selDecision);
            return b;
        }
//GETS

        [HttpGet("betweendates")]
        public async Task<ActionResult<IReadOnlyList<SelDecision>>> GetSelDecisionsBetweenDates(DateTime date1, DateTime date2, enumSelectionResult status)
        {
            var sel = await _selService.GetSelDecisionBetweenDates(date1, date2, status);
            if (sel==null) return NotFound(new ApiResponse(400, "No selection data available for the criteira given"));
            return Ok(sel);
        }

        [HttpGet("enquiryid")]
        public async Task<ActionResult<IReadOnlyList<SelDecision>>> GetSelDecisionsOfAnEnquiry(int[] enquiryId)
        {
            var sel = await _selService.GetSelDecisionsofEnquiryIds(enquiryId);
            if (sel==null) return NotFound(new ApiResponse(400, "No selection data available for the given Demand Letter"));
            return Ok(sel);
        }

        [HttpGet("idwstatus")]
        public async Task<ActionResult<IReadOnlyList<SelDecision>>> GetSelDecisionsOfAnEnquiry(int[] enquiryIds, enumSelectionResult status)
        {
            var sel = await _selService.GetSelDecisionsofEnquiryIdsWithStatus(enquiryIds, status);
            if (sel==null) return NotFound(new ApiResponse(400, "No selection data available for the given criteria"));
            return Ok(sel);
        }

        [HttpGet("itemidwtatus")]
        public async Task<ActionResult<IReadOnlyList<SelDecision>>> GetSelDecisionsOfEnquiryItemsWithStatus(int[] enquiryItemIds, enumSelectionResult status)
        {
            var sel = await _selService.GetSelDecisionsOfEnquiryItemIdsWithStatus(enquiryItemIds, status);
            if (sel==null) return NotFound(new ApiResponse(400, "No selection data available for the given criteria"));
            return Ok(sel);
        }

        
        [HttpGet("withspecs")]
        public async Task<ActionResult<IReadOnlyList<SelDecision>>> GetSelDecisionsWithSpecs(SelDecisionParams param)
        {
            var sel = await _selService.GetSelDecisionsWithSpecs(param);
            if (sel==null) return NotFound(new ApiResponse(400, "No selection data available for the given criteria"));
            return Ok(sel);
        }


    }
}