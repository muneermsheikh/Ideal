using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities.Masters;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Helpers
{
    public class AssessmentQBankToAddDtoCategoryNameResolver : IValueResolver<AssessmentQBank, AssessmentQBankToAddDto, string>
    {
        private readonly ATSContext _context;
        public AssessmentQBankToAddDtoCategoryNameResolver(ATSContext context)
        {
            _context = context;
        }

        public string Resolve(AssessmentQBank source, AssessmentQBankToAddDto destination, string destMember, ResolutionContext context)
        {
            var catName = _context.Categories.Where(x => x.Id==source.CategoryId).Select(x=>x.Name).SingleOrDefault();
            if (catName == null) return "invalid category id";

            return catName;
        }
    }
}