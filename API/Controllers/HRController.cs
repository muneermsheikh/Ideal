using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
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
using Newtonsoft.Json;

namespace API.Controllers
{
    public class HRController : BaseApiController
    {
        private readonly ATSContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICandidateService _candidateService;
        private readonly ICandidateCategoryService _candidateCategoryService;
        public HRController(ATSContext context,
            ICandidateService candidateService, IUnitOfWork unitOfWork,
            ICandidateCategoryService candidateCategoryService, IMapper mapper)
        {
            _candidateCategoryService = candidateCategoryService;
            _candidateService = candidateService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
        }

//candidates
        [Cached(600)]
        [HttpGet("getcandidate/{id}")]
        public async Task<ActionResult<CandidateDto>> GetCandidate(int id)
        {
            var cv = await _candidateService.GetCandidateById(id);  
            if (cv==null) return NotFound();
            
            return Ok( new CandidateDto(cv.Id, cv.ApplicationNo,cv.ApplicationDated,
                    cv.Gender, cv.FullName,  DateTime.Now.Year - cv.DateOfBirth.Year, cv.PassportNo,
                    cv.AadharNo, "city",
                    Enum.GetName(typeof(enumCandidateStatus), cv.CandidateStatus), null));
        }

        [HttpGet("ppnoexists")]
        public async Task<ActionResult<CandidateDto>> PPNoExists(string ppnumber)
        {
            var cv = await _candidateService.PPNumberExists(ppnumber);
            if (cv==null) return NotFound();
            
            return Ok( new CandidateDto(cv.Id,cv.ApplicationNo,cv.ApplicationDated,
                    cv.Gender, cv.FullName,  DateTime.Now.Year - cv.DateOfBirth.Year, cv.PassportNo,
                    cv.AadharNo, cv.City,
                    Enum.GetName(typeof(enumCandidateStatus), cv.CandidateStatus), null));
        }

        [HttpGet("aadharnoexists")]
        public async Task<ActionResult<Candidate>> AadharNoExists(string aadharnumber)
        {
            var cv = await _candidateService.AadharNumberExists(aadharnumber);
            if (cv==null) return NotFound();
            
            return Ok(cv);

        /*    return Ok( new CandidateDto(cv.Id,cv.ApplicationNo,cv.ApplicationDated,
                    cv.Gender, cv.FullName,  DateTime.Now.Year - cv.DOB.Year, cv.PPNo,
                    cv.AadharNo, cv.CandidateAddresses.City,
                    Enum.GetName(typeof(enumCandidateStatus), cv.CandidateStatus), null));
        */
        }
        

        [HttpGet("candexists")]
        public async Task<ActionResult<CandidateDto>> CheckCandidateExists(int appnumber, string ppnumber, string aadharnumber, string email)
        {
            var cv = await _candidateService.CandidateAppNoOrPPNoOrAadharNoOrEmailExist(appnumber, ppnumber, aadharnumber, email);

            if (cv==null) return NotFound();
            
            return Ok( new CandidateDto(cv.Id,cv.ApplicationNo,cv.ApplicationDated,
                    cv.Gender, cv.FullName,  DateTime.Now.Year - cv.DateOfBirth.Year, cv.PassportNo,
                    cv.AadharNo, cv.City,
                    Enum.GetName(typeof(enumCandidateStatus), cv.CandidateStatus), null));
        }
        
        // [Cached(600)]
        [HttpGet("candidates")]     // ***TO DO - nested mapping
        public async Task<ActionResult<Pagination<IReadOnlyList<Candidate>>>> GetCandidates([FromQuery] CandidateParams candidateParams)
        {
            var totalItems = await _unitOfWork.Repository<Candidate>().CountWithSpecAsync(
                new CandidateSpecForCount(candidateParams));

            if (totalItems == 0) return NotFound(new ApiResponse(400, "Your search conditions did not return any candidate"));

            var cvs = await _candidateService.GetCandidatesBySpecs(candidateParams);

            //var data = _mapper.Map<IReadOnlyList<Candidate>, IReadOnlyList<CandidateDto>>(cvs);
            //var data = MapToCandidateDto(cvs);

    /*
            var dtoList = new List<CandidateDto>();
            foreach(var cand in cvs)
            {
                var catList = new List<CategoryNameDto>();
                //var candCategories = cand.CandidateCategories;
                var candCategories= await _context.CandidateCategories.Where(x => x.CandId == cand.Id).ToListAsync();
                // var catString = "";
                if (candCategories !=null || candCategories.Count > 0)
                {
                    foreach(var item in candCategories)
                    {
                        var cat = await _context.Categories.Where(x=>x.Id==item.CatId)
                            .Select(x=> new {x.Id, x.Name}).FirstOrDefaultAsync();
                        catList.Add(new CategoryNameDto(cat.Id, cat.Name));
                        // cadString += cat.Name;
                    }
                }
                var cityname = cand.CandidateAddresses==null ? "undefined": cand.CandidateAddresses.City;
                dtoList.Add(new CandidateDto(cand.Id,cand.ApplicationNo,cand.ApplicationDated,
                    cand.Gender, cand.FullName, DateTime.Now.Year - cand.DOB.Year, cand.PPNo,
                    cand.AadharNo, cityname,
                    Enum.GetName(typeof(enumCandidateStatus), cand.CandidateStatus), catList));
            }
    */
           return Ok(new Pagination<Candidate>(
                candidateParams.PageIndex, candidateParams.PageSize, totalItems, cvs));
        }

        [HttpPost("registercandidate")]
        // ***TO DO - embed candidatecategory in candidate object
        public async Task<ActionResult<Candidate>> RegisterCandidate(Candidate dto)
        {
            // change candidate model - convert candidateaddress to list
            if (dto.CandidateCategories == null || dto.CandidateCategories.Count == 0)
            {
                return BadRequest(new ApiResponse(400, "Candidate profession not defined"));
            }

            var candidateAdded = await _candidateService.RegisterCandidate( dto);

            if (candidateAdded == null) return BadRequest(new ApiResponse(404, "Failed to register the candidate"));
            return Ok(candidateAdded);

        }

        [HttpPost("registercandidates")]
        // ***TO DO - embed candidatecategory in candidate object
        public async Task<ActionResult<IReadOnlyList<Candidate>>> RegisterCandidates([FromBody] IReadOnlyList<Candidate> candidates)
        {
            var candidateAdded = await _candidateService.RegisterCandidates(candidates);

            if (candidateAdded == null) return BadRequest(new ApiResponse(404, "Failed to register the candidate"));
            return Ok(candidateAdded);

        }

        [HttpPut("candidate")]      
        public async Task<ActionResult<Candidate>> UpdateCandidate([FromBody] Candidate candidate)
        {
            if (string.IsNullOrEmpty(candidate.PassportNo) && string.IsNullOrEmpty(candidate.AadharNo))
            { return BadRequest(new ApiResponse(404, "Either PP No or Aadhar Card No must be provided")); }
            // return await _unitOfWork.Repository<Candidate>().UpdateAsync(candidate);
            return await _candidateService.UpdateCandidate(candidate);
        }

        [HttpDelete("candidate")]
        public async Task<int> DeleteCandidate(Candidate candidate)
        {
            return await _unitOfWork.Repository<Candidate>().DeleteAsync(candidate);
        }

//candidate categories
        [Cached(600)]
        [HttpGet("candwithcats")]       //tested
        public async Task<ActionResult<IReadOnlyList<CandidateDto>>> CandidatesMatchingCategories([FromBody] IReadOnlyList<int> categoryIntIds)
        {
            var cands = await _candidateCategoryService.GetCandidatesWithMatchingCategories(categoryIntIds);
            if (cands==null) return NotFound(new ApiResponse(400, "Your instructions did not return any candidates"));
            if (cands.Count==0) return NotFound(new ApiResponse(400, "Your instructions did not return any candidates"));
            var dto =  MapToCandidateDto(cands);
            return Ok(dto);
        }

        [Cached(600)]
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
            string catName="";
            var cat =  await _candidateCategoryService.AddCandidateCategory(candidateId, categoryId, catName);
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
        public async Task<ActionResult<CandidateCategoryDto>> UpdateCandidateCategory(CandidateCategory candidateCategory)
        {
            var cat = await _candidateCategoryService.UpdateCandidateCategory(candidateCategory);
            if (cat==null) return BadRequest(new ApiResponse(404, "Failed to update the category for the candidate"));
            return _mapper.Map<CandidateCategory, CandidateCategoryDto>(cat);
        }

        [HttpPut("categories")]
        public async Task<int> UpdateCandidateCategories(List<CandidateCategory> candCategories)
        {
            return await _candidateCategoryService.UpdateCandidateCategories(candCategories);
        }

        [HttpDelete("DeleteCandidateCategory")]
        public async Task<int> DeleteCandidateCategory(CandidateCategory candidateCategory)
        {
            return await _candidateCategoryService.DeleteCandidatecategory(candidateCategory);
        }

    //sources
        [HttpGet("candidateSources")]
        public async Task<ActionResult<IReadOnlyList<Source>>> GetCandidateSources()
        {
            var srcs = await _candidateService.GetSources();
            if (srcs==null) return NotFound(new ApiResponse(400));

            return Ok(srcs);
        }

    // IProfessions with candCategories
        [Cached(1200)]
        [HttpGet("CandCatWithProf")]
        public async Task<ActionResult<List<CategoryToReturnDto>>> GetCandidateCatsWithProf()
        {
            var cats = await _candidateService.GetCandCatsWithProf();
            return _mapper.Map<List<Category>, List<CategoryToReturnDto>>(cats);
        }

//private
        private async Task<List<CandidateDto>> MapToCandidateDto(IReadOnlyList<Candidate> candidates)
        {
            
            var dtoList = new List<CandidateDto>();
            foreach(var cand in candidates)
            {
                var catList = new List<CategoryNameDto>();
                //var candCategories = cand.CandidateCategories;
                var candCategories= await _context.CandidateCategories
                    .Where(x => x.CandidateId == cand.Id).ToListAsync();
                if (candCategories !=null)
                {
                    foreach(var item in candCategories)
                    {
                        var cat = await _context.Categories.Where(x=>x.Id==item.CategoryId)
                            .Select(x=> new {x.Id, x.Name}).FirstOrDefaultAsync();
                        catList.Add(new CategoryNameDto(cat.Id, cat.Name));
                    }
                }
                var cityname = cand.City;
                dtoList.Add(new CandidateDto(cand.Id,cand.ApplicationNo,cand.ApplicationDated,
                    cand.Gender, cand.FullName, DateTime.Now.Year - cand.DateOfBirth.Year, cand.PassportNo,
                    cand.AadharNo, cityname,
                    Enum.GetName(typeof(enumCandidateStatus), cand.CandidateStatus), catList));
            }

            return dtoList;
        }
    }
}