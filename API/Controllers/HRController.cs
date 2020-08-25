using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class HRController : BaseApiController
    {
        private readonly ATSContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHRService _hrService;
        private readonly ICandidateService _candidateService;
        private readonly ICandidateCategoryService _candidateCategoryService;
        public HRController(ATSContext context,
            ICandidateService candidateService, IUnitOfWork unitOfWork,
            IHRService hrService, ICandidateCategoryService candidateCategoryService, IMapper mapper)
        {
            _candidateCategoryService = candidateCategoryService;
            _candidateService = candidateService;
            _hrService = hrService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpGet("candidates")]     // ***TO DO - nested mapping
        public async Task<ActionResult<Pagination<IReadOnlyList<CandidateDto>>>> GetCandidates([FromBody] CandidateParams candidateParams)
        {
            var totalItems = await _unitOfWork.Repository<Candidate>().CountWithSpecAsync(
                new CandidateSpecForCount(candidateParams));

            if (totalItems == 0) return NotFound(new ApiResponse(400, "Your search conditions did not return any candidate"));

            var cvs = await _candidateService.GetCandidatesBySpecs(candidateParams);
            //var cvs = await _unitOfWork.Repository<Candidate>().GetEntityListWithSpec(
                //new CandidateSpecs(candidateParams));

            var data = _mapper.Map<IReadOnlyList<Candidate>, IReadOnlyList<CandidateDto>>(cvs);

            return Ok(new Pagination<CandidateDto>(
                candidateParams.PageIndex, candidateParams.PageSize, totalItems, data));
        }

        [HttpPost("registercandidate")]
        // ***TO DO - embed candidatecategory in candidate object
        public async Task<ActionResult<Candidate>> RegisterCandidate([FromBody] Candidate candidate)
        {

            if (string.IsNullOrEmpty(candidate.PPNo) && string.IsNullOrEmpty(candidate.AadharNo))
            { return BadRequest(new ApiResponse(404, "Either PP No or Aadhar Card No must be provided")); }

            if (candidate.CandidateCategories.Count == 0) return BadRequest(new ApiResponse(404, "atleast one category must be provided"));

            //var candidateToAdd = _mapper.Map<CandidateToAddDto, Candidate>(candidateAddDto);

            //CandidateCategories object is not written to DB.
            //so candidate obj w/o candidatecategoreis is mapped to CandidateToAddDto
            //the object CandidateCategories is separately passed on to the registerCandidate method

            var candidateAdded = await _candidateService.RegisterCandidate(candidate);

            if (candidateAdded == null) return BadRequest(new ApiResponse(404, "Failed to register the candidate"));
            return Ok(candidateAdded);

        }


        [HttpPut("candidate")]      // *** to do - shift validation to services
        public async Task<ActionResult<Candidate>> UpdateCandidate(Candidate candidate)
        {
            if (string.IsNullOrEmpty(candidate.PPNo) && string.IsNullOrEmpty(candidate.AadharNo))
            { return BadRequest(new ApiResponse(404, "Either PP No or Aadhar Card No must be provided")); }
            return await _unitOfWork.Repository<Candidate>().UpdateAsync(candidate);
        }

        [HttpDelete("candidate")]
        public async Task<int> DeleteCandidate(Candidate candidate)
        {
            return await _unitOfWork.Repository<Candidate>().DeleteAsync(candidate);
        }

//candidate categories
        [HttpGet("candidatecategories/{categoryid}")]       //tested
        public async Task<ActionResult<IReadOnlyList<CandidateDto>>> CandidatesMatchingCategories(IReadOnlyList<int> categoryIntIds)
        {
            var cands = await _candidateCategoryService.GetCandidatesWithMatchingCategories(categoryIntIds);
            if (cands==null) return NotFound(new ApiResponse(400, "Your instructions did not return any candidates"));
            if (cands.Count==0) return NotFound(new ApiResponse(400, "Your instructions did not return any candidates"));
            return Ok(cands);
        }

        [HttpGet("categoriesofcandidate/{candidateid}")]        //tested
        public async Task<ActionResult<IReadOnlyList<CategoryToReturnDto>>> CandidateCategories(int candidateId)
        {
            var cats = await _candidateCategoryService.GetCandidateCategories(candidateId);
            if (cats==null || cats.Count==0) return NotFound(new ApiResponse(400, "No categories found registered with the selected candidate"));
            return Ok(_mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(cats));
        }

        [HttpPost("addCandidateCategory")]      //tested
        public async Task<ActionResult<CandidateCategory>> AddCandidateCategory(int candidateId, int categoryId)
        {
            var cat =  await _candidateCategoryService.AddCandidateCategory(candidateId, categoryId);
            if (cat==null) return BadRequest(new ApiResponse(404, "failed to insert the category for the candidate"));
            return cat;
        }

        [HttpPost("addCandidateCategories")]    //tested
        public async Task<ActionResult<IReadOnlyList<CandidateCategory>>> AddCandidateCategories(IReadOnlyList<CandidateCategory> candidateCategories)
        {
            var cat =  await _candidateCategoryService.AddCandidateCategories(candidateCategories);
            if (cat==null || cat.Count==0) return BadRequest(new ApiResponse(404, "failed to insert the category for the candidate"));
            return Ok(cat);
        }

        [HttpPut("updateCandidateCategory")]    //tested
        public async Task<ActionResult<CandidateCategory>> UpdateCandidateCategory(CandidateCategory candidateCategory)
        {
            var cat = await _candidateCategoryService.UpdateCandidateCategory(candidateCategory);
            if (cat==null) return BadRequest(new ApiResponse(404, "Failed to update the category for the candidate"));
            return cat;
        }

        [HttpDelete("DeleteCandidateCategory")]
        public async Task<int> DeleteCandidateCategory(CandidateCategory candidateCategory)
        {
            return await _candidateCategoryService.DeleteCandidatecategory(candidateCategory);
        }


    }
}