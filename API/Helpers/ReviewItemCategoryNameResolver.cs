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
        private readonly IUnitOfWork _unitOfWork;
        public ReviewItemCategoryNameResolver(ICategoryService categoryService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        public string Resolve(ContractReviewItem source, ReviewItemDto destination, string destMember, ResolutionContext context)
        {
            var item = _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(source.EnquiryItemId).Result;
            if (item==null) return null;
            var enq = _unitOfWork.Repository<Enquiry>().GetByIdAsync(item.EnquiryId).Result;
            if (enq==null) return null;
            var cat = _unitOfWork.Repository<Category>().GetByIdAsync(item.CategoryItemId).Result;
            return enq.EnquiryNo + "-" + item.SrNo + "-" + cat.Name;
        }
    }
}