namespace Core.Entities.Processing
{
    public class ProcessingStatus: BaseEntity
    {
        public ProcessingStatus()
        {
        }

        public string Name { get; set; }
        public int NextStatusId { get; set; }
    }
}