using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;

namespace API.Helpers
{
    public class EnquiryForwardedCustomerResolver : IValueResolver<EnquiryForwarded, EnquiryForwardedInBriefDto, CustomerInBriefDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public EnquiryForwardedCustomerResolver(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public CustomerInBriefDto Resolve(EnquiryForwarded source, EnquiryForwardedInBriefDto destination, CustomerInBriefDto destMember, ResolutionContext context)
        {
            var cust = _unitOfWork.Repository<Customer>().GetByIdAsync(source.EnquiryId).Result;
            if (cust == null) return null;
            return _mapper.Map<Customer, CustomerInBriefDto>(cust);
        }
    }
}