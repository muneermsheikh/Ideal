using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {

        private readonly IGenericRepository<Category> _catRepo;
        private readonly IGenericRepository<SkillLevel> _skillRepo;
        private readonly IGenericRepository<IndustryType> _indRepo;
        private readonly IMapper _mapper;

        public CategoriesController(IGenericRepository<Category> catRepo,
                IGenericRepository<SkillLevel> skillRepo,
                IGenericRepository<IndustryType> indRepo, IMapper mapper)
        {
            _mapper = mapper;
            _indRepo = indRepo;
            _skillRepo = skillRepo;
            _catRepo = catRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<CategoryToReturnDto>>> GetCategories(
            [FromQuery]CategorySpecsParams catParams)
        {
            var spec = new CategorySpecs(catParams);
            var countSpec = new CategorySpecsCount(catParams);
            var totalItems = await  _catRepo.CountWithSpecAsync(countSpec);

            var cats = await _catRepo.ListWithSpecAsync(spec);

            var data = _mapper
                .Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(cats);
                
            return Ok(new Pagination<CategoryToReturnDto>
                (catParams.PageIndex, catParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        // following 2 for swagger 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryToReturnDto>> GetCategory(int id)
        {
            var spec = new CategorySpecs(id);
            
            var cat = await _catRepo.GetEntityWithSpec(spec);
            if (cat == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Category, CategoryToReturnDto>(cat);
        }

        [HttpGet("indtypes")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<IndustryType>>> GetIndustryTypes()
        {
            var inds = await _indRepo.ListAllAsync();
            if (inds == null) return NotFound(new ApiResponse(404, "No industry types data on record"));
            return Ok(inds);
        }

        [HttpGet("skilllevels")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<SkillLevel>>> GetSkillLevels()
        {
            var levels = await _skillRepo.ListAllAsync();
            if (levels == null) return NotFound(new ApiResponse(404, "No Skill level data found on record"));
            return Ok(levels);
        }

    }
}