namespace Core.Entities.Masters
{
    public class Skill: BaseEntity
    {
        public Skill() {
        }
        
        public int EmployeeId {get; set;}
        public string SkillName {get; set;}
        public string ExpInYears {get; set;}
        public string Proficiency {get; set;}
    }
}