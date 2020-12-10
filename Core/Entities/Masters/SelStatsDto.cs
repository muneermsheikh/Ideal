namespace Core.Entities.Masters
{
    public class SelStatsDto
    {
        public SelStatsDto(int enquiryId, int srNo, string categoryName, int hRExecutiveId, string hRExecName, int selectionCount)
        {
            EnquiryId = enquiryId;
            SrNo = srNo;
            CategoryName = categoryName;
            HRExecutiveId = hRExecutiveId;
            HRExecName = hRExecName;
            SelectionCount = selectionCount;
        }

        public int EnquiryId {get; set; }
        public int SrNo {get; set; }
        public string CategoryName {get; set; }
        public int HRExecutiveId {get; set; }
        public string HRExecName {get; set;}
        public int SelectionCount {get; set;}
    }
}