using System.ComponentModel.DataAnnotations;

namespace Core.Entities.EnquiryAggregate
{
   public class JobDesc: BaseEntity
    {
        public int EnquiryItemId { get; set; }
        public string JobDescription { get; set; }
        public string QualificationDesired { get; set; }
        [Range(0,25)]
        public int ExperienceDesiredMin { get; set; }
        [Range(0,35)]
        public int ExperienceDesiredMax { get; set; }
        public string JobProfileDetails { get; set; }
        public string JobProfileUrl { get; set; }
    }
}