using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.Masters;

namespace Core.Specifications
{
    public class EmployeeSpecs : BaseSpecification<Employee>
    {
        public EmployeeSpecs(EmployeeParam eParam) 
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
            AddInclude(x => x.Addresses);
            AddInclude(x => x.Skills);

            ApplyPaging(eParam.PageSize * (eParam.PageIndex-1), eParam.PageSize);

            if (!string.IsNullOrEmpty(eParam.Sort))
            {
                switch (eParam.Sort)
                {
                    case "DesignationAsc":
                        AddOrderBy(x => x.Designation);
                        break;
                    case "DesignationDesc":
                        AddOrderByDescending(x => x.Designation);
                        break;
                    case "GenderAsc":
                        AddOrderBy(x => x.Gender);
                        break;
                    case "GenderDesc":
                        AddOrderByDescending(x => x.Gender);
                        break;
                    case "EmailAsc":
                        AddOrderBy(x => x.Email);
                        break;
                    case "EmailDesc":
                        AddOrderByDescending(x => x.Email);
                        break;
                    case "FirstNameAsc":
                        AddOrderBy(x => x.FirstName);
                        break;
                    case "FirstNameDesc":
                        AddOrderByDescending(x => x.FirstName);
                        break;
                    case "FamilyNameAsc":
                        AddOrderBy(x => x.FamilyName);
                        break;
                    case "FamilyNameDesc":
                        AddOrderBy(x => x.FamilyName);
                        break;
                    default:
                        AddOrderBy(x => x.FirstName);
                        break;
                }
            }

            
        }

        public EmployeeSpecs(int employeeId) 
            : base(x => x.Id == employeeId)
        {
            AddInclude(x => x.Addresses);
        }
    }
}