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

        
        public JobDesc(string jobDescription, string jobProfileUrl) 
        {
            JobDescription = jobDescription;
            JobProfileUrl = jobProfileUrl;
        }

        public JobDesc(int enquiryId, int enquiryItemId, string jobDescription, 
            string jobProfileUrl) 
        {
            EnquiryId = enquiryId;
            EnquiryItemId = enquiryItemId;
            JobDescription = jobDescription;
            JobProfileUrl = jobProfileUrl;
        }

        public int EnquiryId {get; set; }
        public int EnquiryItemId { get; set; }
        public string JobDescription { get; set; } = "not defined";
        public string QualificationDesired { get; set; } = "not defined";
        [Range(0,25)]
        public int ExperienceDesiredMin { get; set; } =0;
        [Range(0,35)]
        public int ExperienceDesiredMax { get; set; } = 0;
        public string JobProfileDetails { get; set; }= "not defined";
        public string JobProfileUrl { get; set; }= "not defined";
    }
}