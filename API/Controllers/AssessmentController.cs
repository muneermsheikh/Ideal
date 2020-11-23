using System;
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
        [HttpGet("assess/{itemid}/{candidateid}")]
        public async Task<ActionResult<AssessmentDto>> CreateAssessmentSheet(int itemid, int candidateid)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var candidateAssessment = await _assessService.CreateAssessment(itemid, candidateid, user.DisplayName);
            if (candidateAssessment==null) return null;
            //var s= await MapAssessmenttoAssessmentDto(candidateAssessment);
            return Ok(candidateAssessment);
            //return candidateAssessment;
        }

        [HttpPost("assess")]
        public async Task<ActionResult<AssessmentDto>> UpdateAssessment(Assessment assessment)
        {
            var assessed = await _assessService.UpdateAssessment(assessment);
            if (assessed==null) return BadRequest(new ApiResponse(404, "failed to update the assessment sheet"));
            return Ok( MapAssessmenttoAssessmentDto(assessed));
        }

        [HttpGet("sheet/{itemid}/{id}")]
        public async Task<ActionResult<AssessmentDto>> GetCandidateAssessmentSheet(int itemid, int id)
        {
            var sheet = await _unitOfWork.Repository<Assessment>().GetEntityWithSpec(new AssessmentSpec(id, itemid));
            if (sheet == null) return NotFound(new ApiResponse(404));
            
            return Ok(_mapper.Map<Assessment, AssessmentDto>(sheet));
        }


        [HttpGet("assessmentsheet/{assessmentid}")]
        public async Task<ActionResult<AssessmentDto>> GetCandidateAssessmentSheet(int assessmentid)
        {
            var sheet = await _unitOfWork.Repository<Assessment>().GetByIdAsync(assessmentid);
            if (sheet == null) return NotFound(new ApiResponse(400, "No assessments available for candidate chosen"));
            return Ok(_mapper.Map<Assessment, AssessmentDto>(sheet));
        }

    //assessment q for enqiry items
        [HttpPost("specqforitem/{id}")]
        public async Task<ActionResult<AssessmentQDto>> CreateAssessmentQForEnquiryItem(int id)
        {
            var Qs = await _assessService.CopyQToAssessmentQOfEnquiryItem(id);
            var LstHdr = MapAssessmentQtoAssessmentQDto(id, Qs);
            return Ok(LstHdr);
        }

        [HttpPost("stddqforitem/{id}")]
        public async Task<ActionResult<AssessmentQDto>> GenerateStddAssessmentQForEnquiryItem(int id)
        {
            var Qs = await _assessService.CopyStddQToAssessmentQOfEnquiryItem(id);
            if (Qs == null || Qs.Count == 0) return BadRequest(new ApiResponse(400, "Failed to copy standard assessment Questions"));

            var LstHdr = MapAssessmentQtoAssessmentQDto(id, Qs);
            return Ok(LstHdr);
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
        public async Task<ActionResult<AssessmentQForCategoryDto>> GetQBankForACategory(int id)
        {
            var qs = await _assessService.GetQBankForACategory(id);
            //var Qs = _mapper.Map<IReadOnlyList<AssessmentQBank>, IReadOnlyList<AssessmentQBankToAddDto>>(qs);
            var LstHdr = MapAssessmentQBankForCategorytoAssessmentQDto(id, qs);
            return Ok(LstHdr);

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

        private async Task<AssessmentQForCategoryDto> MapAssessmentQBankForCategorytoAssessmentQDto(int categoryId, IReadOnlyList<AssessmentQBank> Qs)
        {
            /* var item = await _context.EnquiryItems.Where(x => x.CategoryItemId == categoryId)
                .Select(x => new { x.EnquiryId, x.SrNo, x.CategoryItemId }).FirstOrDefaultAsync();
            var enq = await _context.Enquiries.Where(x => x.Id == item.EnquiryId)
                .Select(x => new { x.EnquiryNo, x.EnquiryDate, x.Customer.CustomerName }).FirstOrDefaultAsync();
            */
            var cat = await _context.Categories.Where(x => x.Id == categoryId).Select(x => x.Name).FirstOrDefaultAsync();

            var LstHdr = new AssessmentQForCategoryDto();
            LstHdr.Category = cat;
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

        private async Task<AssessmentDto> MapAssessmenttoAssessmentDto(Assessment assessment)
        {
            /*
            var item = await _context.EnquiryItems.Where(x => x.Id == assessment.EnquiryItemId)
                .Select(x => new { x.EnquiryId, x.SrNo, x.CategoryItemId }).FirstOrDefaultAsync();
            var enq = await _context.Enquiries.Where(x => x.Id == item.EnquiryId)
                .Select(x => new { x.EnquiryNo, x.EnquiryDate, x.Customer.CustomerName }).FirstOrDefaultAsync();
            var cat = await _context.Categories.Where(x => x.Id == item.CategoryItemId).Select(x => x.Name).FirstOrDefaultAsync();
            */
            if (assessment.CandidateId==0 || assessment.EnquiryItemId==0) return null;

            var qry = await (from i in _context.EnquiryItems join e in _context.Enquiries
                on i.EnquiryId equals e.Id 
                join c in _context.Categories on i.CategoryItemId equals c.Id 
                where i.Id == assessment.EnquiryItemId 
                select new { enquiryno = e.EnquiryNo, enquirydate=e.EnquiryDate,
                    srno=i.SrNo, cat = c.Name, customername=e.Customer.CustomerName,
                    city=e.Customer.City})
                .FirstOrDefaultAsync();
            var cnd = await _context.Candidates.Where(x=>x.Id==assessment.CandidateId)
                .Select(x => new {appno = x.ApplicationNo, name=x.FullName}).FirstOrDefaultAsync();
            var LstHdr = new AssessmentDto();
            LstHdr.EnquiryItemId = assessment.EnquiryItemId;
            LstHdr.CategoryNameAndRef = qry.enquiryno + "-" + qry.srno + "-" + qry.cat;
            LstHdr.OrderNoAndDate = qry.enquiryno + "/" + qry.enquirydate;
            LstHdr.CustomerNameAndCity = qry.customername + ", " + qry.city;
            LstHdr.CandidateName = cnd.name;
            LstHdr.ApplicationNo = cnd.appno;
            LstHdr.AssessedBy=assessment.AssessedBy;
            LstHdr.GradeString= LstHdr.GradeString + " %ge (" + assessment.GradeString + ")";
            LstHdr.Result = Enum.GetName(typeof(enumAssessmentResult), assessment.Result);
 
            LstHdr.TotalCount = assessment.AssessmentItems.Count;

            var lst = new List<AssessmentItemDto>();
            int j = 0;
            foreach (var q in assessment.AssessmentItems)
            {
                lst.Add(new AssessmentItemDto(++j, q.IsMandatory, q.Assessed, q.AssessmentParameter,
                    q.Question, q.MaxPoints, q.PointsAllotted, q.AssessmentId));
            }

            LstHdr.Assessments = lst;

            return LstHdr;
        }
    }
}