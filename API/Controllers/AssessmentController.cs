using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class AssessmentController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssessmentService _assessService;
        private readonly IMapper _mapper;
        private readonly ATSContext _context;
        private readonly UserManager<AppUser> _userManager;
        public AssessmentController(IUnitOfWork unitOfWork, IAssessmentService assessService, ATSContext context, UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _assessService = assessService;
            _unitOfWork = unitOfWork;
        }

        //assessment of candidates
        [HttpPost("assessment")]
        public async Task<ActionResult<Assessment>> CreateCandidateAssessment(int enquiryItemId, int candidateId)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var candidateAssessment = await _assessService.CreateAssessment(enquiryItemId, candidateId, user.DisplayName);
            if (candidateAssessment==null) return null;
            return candidateAssessment;
        }

        [HttpPut("updateassessment")]
        public async Task<ActionResult<Assessment>> UpdateAssessment(AssessmentToAddDto assessmentAdd)
        {
            var assess = _mapper.Map<AssessmentToAddDto, Assessment>(assessmentAdd);
            if (assess == null) return BadRequest(new ApiResponse(400));
            var assessed = await _assessService.UpdateAssessment(assess);
            return Ok(await _unitOfWork.Repository<Assessment>().AddAsync(assess));
        }

        [HttpGet("assessmentsheet")]
        public async Task<ActionResult<Assessment>> GetCandidateAssessmentSheet(
            int CandidateId, int EnquiryItemId)
        {
            var sheet = await _unitOfWork.Repository<Assessment>().GetEntityWithSpec(
                new AssessmentSpec(CandidateId, EnquiryItemId));
            if (sheet == null) return NotFound(new ApiResponse(404));
            return Ok(sheet);
        }


        [HttpGet("assessmentsheet/{id}")]
        public async Task<ActionResult<Assessment>> GetCandidateAssessmentSheet(int id)
        {
            return await _unitOfWork.Repository<Assessment>().GetByIdAsync(id);
        }

        //assessment q for enqiry items
        [HttpPost("enquiryitemqspec/{id}")]
        public async Task<ActionResult<AssessmentQDto>> CreateAssessmentQForEnquiryItem(int id)
        {
            var Qs = await _assessService.CopyQToAssessmentQOfEnquiryItem(id);
            var LstHdr = MapAssessmentQtoAssessmentQDto(id, Qs);
            return Ok(LstHdr);
        }

        [HttpPost("enquiryitemqstdd/{id}")]
        public async Task<ActionResult<AssessmentQDto>> GenerateStddAssessmentQForEnquiryItem(
                int id)
        {
            var Qs = await _assessService.CopyStddQToAssessmentQOfEnquiryItem(id);
            if (Qs == null || Qs.Count == 0) return BadRequest(new ApiResponse(400, "Failed to copy standard assessment Questions"));

            var LstHdr = MapAssessmentQtoAssessmentQDto(id, Qs);
            return Ok(LstHdr);
            /*
            var lst = new List<AssessmentQItemDto>();
            int j = 0;
            foreach (var q in Qs)
            {
                lst.Add(new AssessmentQItemDto(++j, q.IsMandatory, q.AssessmentParameter,
                    q.Question, q.MaxPoints));
            }
            
            var assessQ = await (from e in _context.Enquiries 
                join i in _context.EnquiryItems on e.Id equals i.EnquiryId
                join c in _context.Categories on i.CategoryItemId equals c.Id 
                where i.Id == id 
                select new {enqno = e.EnquiryNo, enqdt = e.EnquiryDate, 
                    srno=i.SrNo, catname=c.Name}).SingleOrDefaultAsync();
            

            var item  = await _context.EnquiryItems.Where(x => x.Id == id)
                .Select(x => new {x.EnquiryId, x.SrNo, x.CategoryItemId}).FirstOrDefaultAsync();
            var enq = await _context.Enquiries.Where(x=>x.Id==item.EnquiryId)
                .Select(x=> new {x.EnquiryNo, x.EnquiryDate, x.Customer.CustomerName}).FirstOrDefaultAsync();
            var cat = await _context.Categories.Where(x=>x.Id==item.CategoryItemId).Select(x=>x.Name).FirstOrDefaultAsync();
            
            var LstHdr = new AssessmentQDto();
            LstHdr.EnquiryItemId = id;
            LstHdr.CategoryRef=enq.EnquiryNo+"-"+item.SrNo+"-"+cat;
            LstHdr.EnquiryNoAndDate=enq.EnquiryNo + "/" + enq.EnquiryDate;
            LstHdr.CustomerName=enq.CustomerName;
            LstHdr.AssessmentQItemListDto = lst;

            return Ok(LstHdr);
            */
        }

        [HttpPut("qforenquiryitem")]
        public async Task<ActionResult<int>> UpdateQEnquiryItem(List<AssessmentQ> Qs)
        {
            var recordsAffected = await _assessService.UpdateQsOfEnquiryItem(Qs);
            if (recordsAffected < Qs.Count) return BadRequest(new ApiResponse(404, "Failed to update all questions"));
            return recordsAffected;
        }

        [HttpGet("qforenquiryitem/{id}")]
        public async Task<ActionResult<AssessmentQDto>> GetQForEnquiryItem(int id)
        {
            var qs = await _assessService.GetAssessmentQsOfEnquiryItem(id);
            var LstHdr = MapAssessmentQtoAssessmentQDto(id, qs);
            return Ok(LstHdr);
        }


        //AssessmentQBank
        [HttpPost("assessqbank")]
        public async Task<IReadOnlyList<AssessmentQBankToAddDto>> AddQListToAssessmentQBank([FromBody]
            List<AssessmentQBank> Qs)
        {
            var qs = await _assessService.AddQListToAssessmentQBank(Qs);
            return _mapper.Map<IReadOnlyList<AssessmentQBank>, IReadOnlyList<AssessmentQBankToAddDto>>(qs);
        }

        [HttpPut("assessmentqbank")]
        public async Task<ActionResult<int>> UpdateQListToAssessmentQBank(List<AssessmentQBankToAddDto> Qs)
        {
            var qs = _mapper.Map<List<AssessmentQBankToAddDto>, List<AssessmentQBank>>(Qs);
            var i = await _assessService.UpdateAssessmentQsBank(qs);
            if (i == 0) return BadRequest(new ApiResponse(404, "failed to update the Question Bank data"));
            return i;
        }

        [HttpGet("qbankforcategory/{id}")]
        public async Task<ActionResult<AssessmentQBankToAddDto>> GetQBankForACategory(int id)
        {
            var qs = await _assessService.GetQBankForACategory(id);
            var Qs = _mapper.Map<IReadOnlyList<AssessmentQBank>, IReadOnlyList<AssessmentQBankToAddDto>>(qs);
            //var LstHdr = MapAssessmentQtoAssessmentQDto(id, qs);
            return Ok(Qs);

        }

        [HttpGet("qforcategories")]
        public async Task<IReadOnlyList<AssessmentQBankToAddDto>> GetQBankAll()
        {
            var qs = await _assessService.GetQBank_All();
            var Qs = _mapper.Map<IReadOnlyList<AssessmentQBank>, IReadOnlyList<AssessmentQBankToAddDto>>(qs);
            return Qs;
        }

        // private
        private async Task<AssessmentQDto> MapAssessmentQtoAssessmentQDto(int id, IReadOnlyList<AssessmentQ> Qs)
        {
            var item = await _context.EnquiryItems.Where(x => x.Id == id)
                .Select(x => new { x.EnquiryId, x.SrNo, x.CategoryItemId }).FirstOrDefaultAsync();
            var enq = await _context.Enquiries.Where(x => x.Id == item.EnquiryId)
                .Select(x => new { x.EnquiryNo, x.EnquiryDate, x.Customer.CustomerName }).FirstOrDefaultAsync();
            var cat = await _context.Categories.Where(x => x.Id == item.CategoryItemId).Select(x => x.Name).FirstOrDefaultAsync();

            var LstHdr = new AssessmentQDto();
            LstHdr.EnquiryItemId = id;
            LstHdr.CategoryRef = enq.EnquiryNo + "-" + item.SrNo + "-" + cat;
            LstHdr.EnquiryNoAndDate = enq.EnquiryNo + "/" + enq.EnquiryDate;
            LstHdr.CustomerName = enq.CustomerName;
            LstHdr.CountOfItems = Qs.Count;

            var lst = new List<AssessmentQItemDto>();
            int j = 0;
            foreach (var q in Qs)
            {
                lst.Add(new AssessmentQItemDto(++j, q.IsMandatory, q.AssessmentParameter,
                    q.Question, q.MaxPoints));
            }

            LstHdr.AssessmentQItemListDto = lst;

            return LstHdr;
        }

    }
}