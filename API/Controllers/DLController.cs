using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;
using Core.Entities.Admin;
using Core.Entities.Dtos;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

// This controller contains all aspects of the Enquiry object after it is contract reviewed and
// approved - i.e. it becomes an Order (called Demand Letter in overseas recruitment terms).
// this controller is accessed only by company staff.
namespace API.Controllers
{
    public class DLController : BaseApiController
    {
        private readonly IGenericRepository<EnquiryForwarded> _enqFwdRepo;
        private readonly IEnquiryService _enqService;
        private readonly IDLForwardService _dlForwardService;

        public DLController(IEnquiryService enqService,
            IGenericRepository<EnquiryForwarded> enqFwdRepo,
            IDLForwardService dlForwardService)
        {
            _dlForwardService = dlForwardService;
            _enqService = enqService;
            _enqFwdRepo = enqFwdRepo;
        }

    // send requirement to HR Department


    // enquiryies forwarded

    [HttpPost("enqforward/{mode}")]
    public async Task<IReadOnlyList<EnquiryForwarded>> SendEnquiryIdToAssociates(
        DateTimeOffset dtForwarded, int enquiryId, string sMode, 
        IReadOnlyList<CustOfficialToForwardDto> officialDto)
    {
        return await _dlForwardService.CreateEnquiryForwardForEnquiryIdAsync(
            officialDto, enquiryId, sMode, dtForwarded);
    }

    [HttpGet("enqforwardlist")]
    public async Task<ActionResult<Pagination<EnquiryForwarded>>> GetEnquiriesForwardedList
        ([FromQuery] EnqForwardSpecParams enqParams)
    {
        var spec = new EnqForwardedWithFilterSpec(enqParams);
        var countSpec = new EnqForwardedWithFiltersForCountSpec(enqParams);
        var totalItems = await _enqFwdRepo.CountWithSpecAsync(countSpec);

        var enqFwds = await _enqFwdRepo.ListWithSpecAsync(spec);

        // var data = _mapper
        // .Map<IReadOnlyList<EnquiryForwarded>, IReadOnlyList<API.Dtos.EnqForwardedDto>>(enqFwds);
        return Ok(new Pagination<EnquiryForwarded>
            (enqParams.PageIndex, enqParams.PageSize, totalItems, enqFwds));
    }

}
}