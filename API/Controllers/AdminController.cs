using System;
using System.Collections.Generic;
using System.Linq;
// using System.Text.Json;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    //[Authorize]
    public class AdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ATSContext _context;
        private readonly ICVRefService _cvRefService;
        private readonly IAdminServices _admnServices;

        
        public AdminController(ATSContext context,
            UserManager<AppUser> userManager, IMapper mapper, ICVRefService cvRefService, IAdminServices admnServices)
        {
            _admnServices = admnServices;
            _cvRefService = cvRefService;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<ActionResult<CVForward>> ReferCVs([FromBody] CVRefToAddDto cvrefdto)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            int userId = 0;
            if (user==null) 
            {
                userId=1;
            }
            else {
                 userId = await _context.Employees.Where(x => x.Email == user.Email)
                .Select(x => x.Id).FirstOrDefaultAsync();
            }
            
            var forwarded = await _cvRefService.ReferCVsToClient(userId, cvrefdto);

            if (forwarded == null) return BadRequest(new ApiResponse(400, "failed to register CV referrals")); ;
            return Ok(forwarded);

        }

        [HttpGet]
        public async Task<ActionResult<List<CVForwardDto>>> GetCVForwards(int enquiryno)
        {

            var cvfwd = await _cvRefService.GetCVForwards(enquiryno); 
            if (cvfwd==null || cvfwd.Count ==0) return NotFound(new ApiResponse(404, "No records exist matching your criteria"));
            
            
            return Ok(_mapper.Map<IReadOnlyList<CVForward>, IReadOnlyList<CVForwardDto>>(cvfwd));
        }

        [HttpGet("pendingrequirementofItems")]
        public ActionResult<IReadOnlyList<RequirementPendingDto>> GetPendingRequirementsOfEnquiryItems([FromBody] int[] enquiryitems)
        {
            var reqmts = _admnServices.PendingRequirements(enquiryitems);
            return Ok(reqmts);
        }

        [HttpGet("pendingrequirementallItems")]
        public ActionResult<IReadOnlyList<RequirementPendingDto>> GetPendingRequirementsOfEnquiryItems()
        {
            var reqmts = _admnServices.PendingRequirements();
            return Ok(reqmts);
        }


        [HttpGet("cvforwardedbyenqids")]
        public async Task<ActionResult<IReadOnlyList<CVForward>>> CVForwardedDetails(int[] enquiryIds)
        {
            return Ok(await _admnServices.CVForwardDetailsOfDLs(enquiryIds));
        }

        
    }
}