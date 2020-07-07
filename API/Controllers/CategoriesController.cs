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
        public async Task<ActionResult<Pagination<CategoryToReturnDto>>> GetCategories([FromQuery]CategorySpecParams catParams)
        {
            var spec = new CategoryWithIndTypeAndSkillLevelSpec(catParams);
            var countSpec = new CategoriesWithFiltersForCountSpec(catParams);
            var totalItems = await _catRepo.CountAsync(countSpec);

            var cats = await _catRepo.ListAsync(spec);

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
            var spec = new CategoryWithIndTypeAndSkillLevelSpec(id);
            
            var cat = await _catRepo.GetEntityWithSpec(spec);
            if (cat == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Category, CategoryToReturnDto>(cat);
        }

        [HttpGet("indtypes")]
        public async Task<ActionResult<IReadOnlyList<IndustryType>>> GetIndustryTypes()
        {
            var inds = await _indRepo.ListAllAsync();
            return Ok(inds);
        }

        [HttpGet("skilllevels")]
        public async Task<ActionResult<IReadOnlyList<SkillLevel>>> GetSkillLevels()
        {
            var levels = await _skillRepo.ListAllAsync();
            return Ok(levels);
        }
    }
}