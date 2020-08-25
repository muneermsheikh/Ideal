using System.Collections.Generic;
using API.Dtos;
using AutoMapper;
using Core.Entities.Admin;
using Core.Interfaces;

namespace API.Helpers
{
    public class EnquiryForwardedAssociateResolver : IValueResolver<EnquiryForwarded, 
        EnquiryForwardedDto, CustomerInBriefDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EnquiryForwardedAssociateResolver(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public CustomerInBriefDto Resolve(EnquiryForwarded source, 
            EnquiryForwardedDto destination, CustomerInBriefDto destMember, 
            ResolutionContext context)
        {
            var offs = new List<CustomerOfficial>();
            if (source.CustomerOfficialId == 0 || source.CustomerId == 0) return null;
            var cust = _unitOfWork.Repository<Customer>().GetByIdAsync(source.CustomerId).Result;
            if (cust==null) return null;
            var off = _unitOfWork.Repository<CustomerOfficial>().GetByIdAsync(source.CustomerOfficialId).Result;
            if (off==null) return null;
            offs.Add(off);
            cust.CustomerOfficials=offs;
            return _mapper.Map<Customer, CustomerInBriefDto>(cust);
        }
    }
}