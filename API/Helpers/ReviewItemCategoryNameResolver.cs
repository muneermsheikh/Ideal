using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Interfaces;

namespace API.Helpers
{
    public class ReviewItemCategoryNameResolver : IValueResolver<ContractReviewItem, ReviewItemDto, string>
    {
        private readonly ICategoryService _categoryService;
        public ReviewItemCategoryNameResolver(ICategoryService categoryService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
        }

        public string Resolve(ContractReviewItem source, ReviewItemDto destination, string destMember, ResolutionContext context)
        {
            return _categoryService.GetCategoryNameWithRefFromEnquiryItemId(source.EnquiryItemId);
        }
    }
}