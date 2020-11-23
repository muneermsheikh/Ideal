using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;

namespace API.Helpers
{
    public class EnquiryTotalItemsResolver : IValueResolver<Enquiry, EnquiryWithAllStatusDto, int>
    {
        public int Resolve(Enquiry source, EnquiryWithAllStatusDto destination, int destMember, ResolutionContext context)
        {
            var items = source.EnquiryItems;
            if(items != null)
            {
                return items.Count;
            }
            return 0;
        }
    }
}