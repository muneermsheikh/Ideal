namespace API.Dtos
{
    public class JobDescDto
    {
        public int Id {get; set; }
        public int EnquiryId {get; set; }
        public int EnquiryItemId { get; set; }
        public string JobDescription { get; set; } 
        public string QualificationDesired { get; set; } 
        public int ExperienceDesiredMin { get; set; } 
        public int ExperienceDesiredMax { get; set; }
        public string JobProfileDetails { get; set; }
        public string JobProfileUrl { get; set; }
    }
}