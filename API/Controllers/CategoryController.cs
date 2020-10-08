using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;

namespace API.Controllers
{
    // [Authorize]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IGenericRepository<Category> _catRepo;
        private readonly IGenericRepository<IndustryType> _indRepo;
        private readonly IGenericRepository<SkillLevel> _skillRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(
            ICategoryService categoryService,
            IGenericRepository<Category> catRepo, IGenericRepository<IndustryType> indRepo,
            IGenericRepository<SkillLevel> skillRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _catRepo = catRepo;
            _indRepo = indRepo;
            _skillRepo = skillRepo;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<IReadOnlyList<CategoryToReturnDto>>>> GetCategories([FromQuery] CategorySpecsParams catParams)
        {
            var spec = new CategorySpecs(catParams);
            var CtSpec = new CategorySpecsCount(catParams);
            var totalItems = await  _catRepo.CountWithSpecAsync(CtSpec);
            var cats = await _catRepo.ListWithSpecAsync(spec);
        
            if (cats==null) return NotFound(new ApiResponse(404, 
                "Your criteria did not return any category records"));
            
            var data = _mapper.Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(cats);
            
            return Ok(new Pagination<CategoryToReturnDto>(
                catParams.PageIndex, catParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryToReturnDto>> GetCategory(int id)
        {
            var spec = new CategorySpecs(id);
            var cat = await _catRepo.GetEntityWithSpec(spec);

            // var cat = await _catRepo.GetByIdAsync(id);
            if (cat == null) return NotFound(new ApiResponse(404, "no records found"));
            var catReturn = _mapper.Map<Category, CategoryToReturnDto>(cat);
            return Ok(catReturn);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryToReturnDto>>AddACategory([FromQuery] CategoryDto categoryDto)
        {
            var cat = await _categoryService.CreateCategoryAsync(
                categoryDto.Name, categoryDto.IndustryTypeId, categoryDto.SkillLevelId);
            if (cat == null) return BadRequest(new ApiResponse(400,"Failed to create the category"));
           // cat = await _categoryService.CategoryByIdAsync(cat.Id);
            var catToReturn = _mapper.Map<Category, CategoryToReturnDto>(cat);
            return Ok(catToReturn);
        }

        [HttpPut("category")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryToReturnDto>> UpdateCategory(Category cat)
        {
            var catUpdated= await _categoryService.UpdateCategoryAsync(cat);
            if (catUpdated==null) return BadRequest(new ApiResponse(400));
            var catToReturn =_mapper.Map<Category, CategoryToReturnDto>(catUpdated);
            return catToReturn;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        public async Task<Category> DeleteCategory(int id)
        {
            return await  _categoryService.DeleteCategoryByIdAsync(id);
        }


        [HttpGet("exists")]
        public async Task<bool> CategoryExists(string cat, int industryTypeId, int skillLevelId)
        {
            return await _categoryService.CategoryExists(cat, industryTypeId, skillLevelId);
        }

// industryType
        [HttpGet("IndustryTypesWoPagination")]
        public async Task<ActionResult<IReadOnlyList<IndustryType>>> GetIndustryTypes()
        {
            return Ok(await _categoryService.GetIndustryTypes());
        }

        [HttpGet("indTypes")]
        public async Task<ActionResult<IReadOnlyList<IndustryType>>> GetIndustryTypesWoPagination ([FromQuery]IndTypeSpecParams param)
        {
            //var inds = await _indRepo.ListAllAsync();
            
            var spec = new IndTypeSpecs(param);
            var ctSpec = new IndTypeSpecsCount(param);
            //var totalItems = await _unitOfWork.Repository<IndustryType>().CountWithSpecAsync(ctSpec);
            var totalItems = await _indRepo.CountWithSpecAsync(ctSpec);
            var inds = await _indRepo.ListWithSpecAsync(spec);
            //var inds = await _unitOfWork.Repository<IndustryType>().ListWithSpecAsync(spec);
            if (inds == null) return NotFound(new ApiResponse(400));

            return Ok(inds);
         }

        [HttpGet("indType")]
        public async Task<ActionResult<Pagination<IReadOnlyList<IndustryType>>>> GetIndustryTypes ([FromQuery]IndTypeSpecParams param)
        {
            //var inds = await _indRepo.ListAllAsync();
            
            var spec = new IndTypeSpecs(param);
            var ctSpec = new IndTypeSpecsCount(param);
            //var totalItems = await _unitOfWork.Repository<IndustryType>().CountWithSpecAsync(ctSpec);
            var totalItems = await _indRepo.CountWithSpecAsync(ctSpec);
            var inds = await _indRepo.ListWithSpecAsync(spec);
            //var inds = await _unitOfWork.Repository<IndustryType>().ListWithSpecAsync(spec);
            if (inds == null) return NotFound(new ApiResponse(400));

            return Ok(new Pagination<IndustryType>(
                param.PageIndex, param.PageSize, totalItems, inds));
         }

         [HttpGet("indType/{id}")]
         public async Task<ActionResult<IndustryType>> GetIndustryTypeById (int id)
         {
             return await _indRepo.GetByIdAsync(id);
         }

/*
        [HttpGet("indType")]
        public async Task<ActionResult<Pagination<IReadOnlyList<IndustryType>>>> GetIndustryTypes ([FromQuery]IndustryTypeParam param)
        {
            var spec = new IndustryTypeSpec(param);
            var ctSpec = new IndustryTypeCount(param);
            //var totalItems = await _unitOfWork.Repository<IndustryType>().CountWithSpecAsync(ctSpec);
            var totalItems = await _indRepo.CountWithSpecAsync(ctSpec);
            var inds = await _indRepo.ListWithSpecAsync(spec);
            //var inds = await _unitOfWork.Repository<IndustryType>().ListWithSpecAsync(spec);
            if (inds == null) return NotFound(new ApiResponse(400));

            return Ok(new Pagination<IndustryType>(
                param.PageIndex, param.PageSize, totalItems, inds));
        }
*/
        
        [HttpPost("indType/{name}")]
        public async Task<IndustryType> CreateIndustryType(string name)
        {
            return await _categoryService.CreateIndustryType(name);
        }

        [HttpPut("indType")]
        public async Task<IndustryType> UpdateIndustryType(IndustryType industryType)
        {
            return await _categoryService.UpdateIndustryType(industryType);
        }

        [HttpDelete("indType/{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        public async Task<IndustryType> DeleteIndustryTypeById(int id)
        {
            return await  _categoryService.DeleteIndustryTypeById(id);
        }

    // skillLevel

        [HttpGet("SkillLevelsWoPagination")]
        public async Task<ActionResult<IReadOnlyList<SkillLevel>>> GetSkillLevelsWoPagination()
        {
            return Ok(await _categoryService.GetSkillLevels());
        }


        [HttpGet("skillLevels")]
        public async Task<ActionResult<IndustryToReturnDto>> GetSkillLevelsWoPaginationNParam(
            [FromQuery]IndTypeSpecParams param)
        {
            var spec = new SkillLevelSpec(param);
            var ctSpec = new SkillLevelCount(param);
            var totalItems = await _unitOfWork.Repository<SkillLevel>().CountWithSpecAsync(ctSpec);
            var sks = await _unitOfWork.Repository<SkillLevel>().ListWithSpecAsync(spec);
            if (sks == null) return NotFound(new ApiResponse(400));
            var data = _mapper.Map<IReadOnlyList<SkillLevel>, 
                IReadOnlyList<IndustryToReturnDto>>(sks);
            return Ok(data);
        }
        
        // no separate paramter SkillLevelParam  necessary, as IndustryTypeParam serves the purpose
        [HttpGet("skillLevel")]
        public async Task<ActionResult<Pagination<IndustryToReturnDto>>> GetSkillLevels(
            [FromQuery]IndTypeSpecParams param)
        {
            var spec = new SkillLevelSpec(param);
            var ctSpec = new SkillLevelCount(param);
            var totalItems = await _unitOfWork.Repository<SkillLevel>().CountWithSpecAsync(ctSpec);
            var sks = await _unitOfWork.Repository<SkillLevel>().ListWithSpecAsync(spec);
            if (sks == null) return NotFound(new ApiResponse(400));
            var data = _mapper.Map<IReadOnlyList<SkillLevel>, 
                IReadOnlyList<IndustryToReturnDto>>(sks);
            return Ok(new Pagination<IndustryToReturnDto>(param.PageIndex,
                param.PageSize, totalItems, data));
        }

        [HttpPost("skillLevel/{name}")]
        public async Task<SkillLevel> CreateSkillLevel(string name)
        {
            return await _categoryService.CreateSkillLevel(name);
        }

        [HttpGet("skillLevel/{id}")]
        public async Task<ActionResult<SkillLevel>> GetSkillLevelById(int id)
        {
            return await _skillRepo.GetByIdAsync(id);
        }
        
        [HttpPut("skillLevel")]
        public async Task<SkillLevel> UpdateSkillLevel(SkillLevel skillLevel)
        {
            return await _categoryService.UpdateSkillLevel(skillLevel);
        }

        [HttpDelete("skillLevel/{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        public async Task<SkillLevel> DeleteSkillLevelById(int id)
        {
            return await  _categoryService.DeleteSkillLevelById(id);
        }
    }
}
