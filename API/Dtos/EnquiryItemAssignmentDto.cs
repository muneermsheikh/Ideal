namespace API.Dtos
{
    public class EnquiryItemAssignmentDto
    {
        public int Id {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int? HRExecId {get; set; }
        public string HRExecName {get; set;}
        public int? AssessingSupId { get; set; }
        public string HRSupName {get; set;}
        public int? AssessingHRMId { get; set; }
        public string HRMName {get; set; }

    }
}