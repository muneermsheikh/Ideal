using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Infrastructure.Data;

namespace API.Helpers
{
    public class EnquiryItemCategoryResolver : IValueResolver<EnquiryItem, EnquiryItemToReturnDto, string>
    {
        private readonly ATSContext _context;
        public EnquiryItemCategoryResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(EnquiryItem source, EnquiryItemToReturnDto destination, string destMember, ResolutionContext context)
        {
            var cat =  _context.Categories.Where(x => x.Id == source.CategoryItemId).Select(x=>x.Name).SingleOrDefault();
            return cat;
        }
    }
}