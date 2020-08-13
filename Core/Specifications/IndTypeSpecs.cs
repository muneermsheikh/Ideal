using System;
using System.Linq.Expressions;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class IndTypeSpecs : BaseSpecification<IndustryType>
    {
        public IndTypeSpecs(IndTypeSpecParams indParams)
            : base(x => 
                (string.IsNullOrEmpty(indParams.Search) || x.Name.ToLower().Contains(indParams.Search)) &&
                (!indParams.Id.HasValue || x.Id == indParams.Id)
            )
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(indParams.PageSize * (indParams.PageIndex-1), indParams.PageSize);
            }

        public IndTypeSpecs(int id) : base(x => x.Id == id)
        {
        }
    }
}