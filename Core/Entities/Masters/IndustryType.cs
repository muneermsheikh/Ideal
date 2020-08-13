namespace Core.Entities.Masters
{
    public class IndustryType: BaseEntity
    {
        public IndustryType()
        {
        }

        public IndustryType(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}