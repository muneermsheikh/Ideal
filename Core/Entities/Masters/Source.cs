namespace Core.Entities.Masters
{
     public class Source: BaseEntity
    {
        public Source()
        {
        }
        public string SourceGroup {get; set; }
        public int SourceGroupId { get; set; }
        public string Name { get; set; }
        
    }
}