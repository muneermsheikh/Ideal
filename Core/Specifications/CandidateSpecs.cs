using System;
using System.Linq.Expressions;
using Core.Entities.HR;

namespace Core.Specifications
{
    public class CandidateSpecs : BaseSpecification<Candidate>
    {
        public CandidateSpecs(CandidateParams cParams) 
            : base(x => 
            (
                (string.IsNullOrEmpty(cParams.Search) || 
                    x.FullName.ToLower().Contains(cParams.Search) ||
                    x.City.ToLower().Contains(cParams.Search) ) &&
                (!cParams.ApplicationNo.HasValue || x.ApplicationNo == cParams.ApplicationNo ) &&
                (!cParams.ApplicationDated.HasValue || DateTime.Compare(
                    x.ApplicationDated.Date, cParams.ApplicationDated.Value.Date) == 0) &&
                (!cParams.Id.HasValue || x.Id == cParams.Id)
            ))
        {
            AddOrderBy(x => x.ApplicationNo);
            if (cParams.includeCategories) AddInclude(x => x.CandidateCategories);
        }

        public CandidateSpecs(int candidateId): base(x => x.Id == candidateId)
        {
        }
    }
}