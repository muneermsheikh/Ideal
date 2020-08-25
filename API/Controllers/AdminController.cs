using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly ATSContext _context;
        private readonly ICVRefService _cvRefService;

        public AdminController(ATSContext context,
            UserManager<AppUser> userManager, IMapper mapper, ICVRefService cvRefService)
        {
            _cvRefService = cvRefService;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<IReadOnlyList<CVRef>>> ReferCVs(int enquiryItemId, 
            List<int> candidateIds, DateTime dateForwarded, bool includePhoto, bool includeGrade, 
            bool includeSalaryExpectation)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            var userId = await _context.Employees.Where(x => x.Email == user.Email).Select(x => x.Id).FirstOrDefaultAsync();
            var referred = await _cvRefService.ReferCVsToClient(userId, enquiryItemId, candidateIds, 
                dateForwarded, includeSalaryExpectation, includeGrade, includePhoto);
            if (referred==null) return BadRequest(new ApiResponse(400, "failed to register CV referrals"));;
            return Ok(referred);
        }
    }
}