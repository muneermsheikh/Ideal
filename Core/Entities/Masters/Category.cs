namespace Core.Entities.Masters
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public IndustryType IndustryType {get; set; }
        public int IndustryTypeId {get; set; }
        public SkillLevel SkillLevel {get; set; }
        public int SkillLevelId {get; set; }
        public string imageUrl {get; set; }
    }
}