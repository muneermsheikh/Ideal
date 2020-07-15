using Core.Entities.Masters;

namespace API.Dtos
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public int IndustryTypeId { get; set; }
        public int SkillLevelId { get; set; }
    }
}