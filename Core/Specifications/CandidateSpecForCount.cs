using System;
using Core.Entities.HR;

namespace Core.Specifications
{
    public class CandidateSpecForCount: BaseSpecification<Candidate>
    {
        public CandidateSpecForCount(CandidateParams cParams) 
            : base(x => 
            (
                (string.IsNullOrEmpty(cParams.Search) || 
                    x.FullName.ToLower().Contains(cParams.Search) || 
                    x.City.ToLower().Contains(cParams.Search)) &&
                (!cParams.ApplicationNo.HasValue || x.ApplicationNo == cParams.ApplicationNo ) &&
                (!cParams.ApplicationDated.HasValue || DateTime.Compare(
                    x.ApplicationDated.Date, cParams.ApplicationDated.Value.Date) == 0) &&
                (!cParams.Id.HasValue || x.Id == cParams.Id)
            ))
        {
        }

        public CandidateSpecForCount(int candidateId): base(x => x.Id == candidateId)
        {
        }
        
    }
}