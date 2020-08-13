using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Interfaces;

namespace API.Helpers
{
    public class ReviewItemReviewedByResolver : IValueResolver<ContractReviewItem, ReviewItemDto, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewItemReviewedByResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Resolve(ContractReviewItem source, ReviewItemDto destination, string destMember, ResolutionContext context)
        {
            int i = source.ReviewedBy;
            if(i != 0)
            {
                var emp = _unitOfWork.Repository<Employee>().GetByIdAsync(i).Result;
                if(emp==null) return null;
                return emp.FirstName;
            }
            return "Not Reviewed";
        }
    }
}