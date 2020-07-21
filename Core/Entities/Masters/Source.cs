namespace Core.Entities.Masters
{
     public class Source: BaseEntity
    {
        public Source()
        {
        }

        public int SourceGroupId { get; set; }
        public string Name { get; set; }
        
    }
}