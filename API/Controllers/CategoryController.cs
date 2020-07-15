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
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        private readonly IGenericRepository<Category> _catRepo;

        private readonly IMapper _mapper;

        public CategoryController(
            ICategoryService categoryService,
            IGenericRepository<Category> catRepo,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _catRepo = catRepo;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<CategoryToReturnDto>>>GetCategories(
            [FromQuery] CategorySpecParams catParams)
        {
            var spec = new CategoryWithIndTypeAndSkillLevelSpec(catParams);
            var countSpec = new CategoriesWithFiltersForCountSpec(catParams);
            var totalItems = await _catRepo.CountWithSpecAsync(countSpec);

            var cats = await _catRepo.ListWithSpecAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Category>, 
                IReadOnlyList<CategoryToReturnDto>>(cats);
            return Ok(new Pagination<CategoryToReturnDto>(catParams.PageIndex,
                catParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategory(int id)
        {
            var cat = await _catRepo.GetByIdAsync(id);
            return Ok(cat);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Category>>AddACategory(string name, int indTypeId, int skillLevelId)
        {
            var cat = await _categoryService.CreateCategoryAsync(name, indTypeId, skillLevelId);
            if (cat == null) return BadRequest(new ApiResponse(400,"Failed to create the category"));
            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public Task<Category> DeleteCategory(int id)
        {
            return  _categoryService.DeleteCategoryByIdAsync(id);
        }
    }
}
