using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;

namespace API.Helpers {
    public class EnquiryQuantitySumResolver : IValueResolver<Enquiry, EnquiryWithAllStatusDto, int>
   {
        public int Resolve(Enquiry source, EnquiryWithAllStatusDto destination, int destMember, ResolutionContext context)
        {
            int total = 0;
            var items = source.EnquiryItems;
            if (items != null)
            {
                foreach(var item in items)
                {
                    total += item.Quantity;
                }
            }
            return total;
        }
    }

}
