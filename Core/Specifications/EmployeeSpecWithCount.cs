using System;
using System.Linq.Expressions;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class EmployeeSpecWithCount : BaseSpecification<Employee>
    {
        public EmployeeSpecWithCount(EmployeeParam eParam) 
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
        }
    }
}