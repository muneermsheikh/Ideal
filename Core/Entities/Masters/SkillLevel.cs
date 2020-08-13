namespace Core.Entities.Masters
{
    public class SkillLevel: BaseEntity
    {
        public SkillLevel()
        {
        }

        public SkillLevel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}