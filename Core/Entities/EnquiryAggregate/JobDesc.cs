using System.ComponentModel.DataAnnotations;

namespace Core.Entities.EnquiryAggregate
{
   public class JobDesc: BaseEntity
    {
        public JobDesc()
        {
        }

        public JobDesc(int enquiryItemId, int enquiryId)
        {
            EnquiryId = enquiryId;
            EnquiryItemId = enquiryItemId;
        }

        public int EnquiryId {get; set; }
        public int EnquiryItemId { get; set; }
        public string JobDescription { get; set; } = "not defined";
        public string QualificationDesired { get; set; } = "not defined";
        [Range(0,25)]
        public int ExperienceDesiredMin { get; set; }
        [Range(0,35)]
        public int ExperienceDesiredMax { get; set; }
        public string JobProfileDetails { get; set; }= "not defined";
        public string JobProfileUrl { get; set; }= "not defined";
    }
}