using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.Dtos;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class EmployeeSpecs : BaseSpecification<Employee>
    {
        public EmployeeSpecs(EmployeeParam eParam) 
            : base(x => 
                (
                    (string.IsNullOrEmpty(eParam.Search) || 
                        x.Person.FullName.ToLower().Contains(eParam.Search)) &&
                    (string.IsNullOrEmpty(eParam.Gender) || 
                        x.Person.Gender == eParam.Gender ) &&
                    (string.IsNullOrEmpty(eParam.Designation) || 
                        x.Person.Gender == eParam.Designation ) &&
                    (!eParam.DOJ.HasValue || DateTimeOffset.Compare(
                        x.DOJ.Date, eParam.DOJ.Value.Date)==0) &&
                    (!eParam.IsInEmployment.HasValue || 
                        x.IsInEmployment==eParam.IsInEmployment) &&
                    (!eParam.Id.HasValue || x.Id==eParam.Id)
                ))
        {
            AddInclude(x => x.Person);
            AddInclude(x => x.EmployeeAddress);
            AddOrderByDescending(x => x.DOJ);        
        }

        public EmployeeSpecs(int employeeId) 
            : base(x => x.Id == employeeId)
        {
            AddInclude(x => x.Person);
            AddInclude(x => x.EmployeeAddress);
        }
    }
}