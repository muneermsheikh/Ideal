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
                    (string.IsNullOrEmpty(eParam.FirstName) || 
                        x.FirstName.ToLower().Contains(eParam.FirstName)) &&
                    (string.IsNullOrEmpty(eParam.FamilyName) ||
                        x.FamilyName.ToLower().Contains(eParam.FamilyName)) &&
                    (string.IsNullOrEmpty(eParam.Designation) ||
                        x.Designation.ToLower().Contains(eParam.Designation)) &&
                    ((string.IsNullOrEmpty(eParam.Email)) || 
                        x.Email.ToLower().Contains(eParam.Email)) &&
                    (string.IsNullOrEmpty(eParam.Gender) || 
                        x.Gender == eParam.Gender ) &&
                    (!eParam.DOJ.HasValue || DateTime.Compare(
                        x.DateOfJoining.Date, eParam.DOJ.Value.Date)==0) &&
                    (!eParam.IsInEmployment.HasValue || 
                        x.IsInEmployment==eParam.IsInEmployment) &&
                    (!eParam.Id.HasValue || x.Id==eParam.Id)
                ))
        {
        }
    }
}