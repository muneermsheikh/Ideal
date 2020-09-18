using System.Collections.Generic;

namespace API.Dtos
{
    public class DLProcessDto
    {
        public int EnquiryId {get; set;}
        public string EnquiryDate {get; set;}
        public string CustomerName {get; set;}
        public List<DLProcessItemDto> DLItemsProcess {get; set;}
    }

    public class DLProcessItemDto
    {
        public int SrNo {get; set; }
        public string CategoryRef {get; set;}
        public int CountOfReferred {get; set;}
        public int CountOfSelected {get; set; }
        public int CountOfRejected {get; set; }
        public int CountOfCVsToForward {get; set;}
    }

}