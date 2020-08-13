using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;

namespace API.Helpers
{
    public class ReviewItemEnquiryNoDateResolver : IValueResolver<ContractReviewItem, ReviewItemDto, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReviewItemEnquiryNoDateResolver(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string Resolve(ContractReviewItem source, ReviewItemDto destination, string destMember, ResolutionContext context)
        {
            var enq = _unitOfWork.Repository<Enquiry>().GetByIdAsync(source.EnquiryId).Result;
            if (enq==null) return null;
            return enq.EnquiryNo + "/" + enq.EnquiryDate.Date;
        }
    }
}