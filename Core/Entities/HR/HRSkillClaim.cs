using Core.Entities.Masters;
using Core.Enumerations;

namespace Core.Entities.HR
{
    public class HRSkillClaim: BaseEntity 
    {
        public HRSkillClaim()
        {
        }

        public int EmployeeId { get; set; }
        public int IndustryTypeId { get; set; }
        public int SkillLevelId { get; set; }
        public string CategoryName { get; set; }
        public Employee Employee {get; set; }
        public IndustryType IndustryType { get; set; }
        public SkillLevel SkillLevel { get; set; }
        
    }
}