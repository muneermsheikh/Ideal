namespace Core.Entities.Masters
{
    public class Category: BaseEntity
    {
        public Category(string name, int industryTypeId, int skillLevelId)
        {
            Name = name;
            IndustryTypeId = industryTypeId;
            SkillLevelId = skillLevelId;
        }

        public string Name { get; set; }
        public IndustryType IndustryType {get; set; }
        public int IndustryTypeId {get; set; }
        public SkillLevel SkillLevel {get; set; }
        public int SkillLevelId {get; set; }

    }
}