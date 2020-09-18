using Core.Enumerations;

namespace Core.Entities.Processing
{
    public class ProcessStatus: BaseEntity
    {
        public ProcessStatus()
        {
        }

        public int SeqId {get; set;}
        public bool Mandatory {get; set;}
        public enumProcessingStatus StatusId {get; set; }
        public string Name { get; set; }
        public enumProcessingStatus NextStatusId { get; set; }
    }
}