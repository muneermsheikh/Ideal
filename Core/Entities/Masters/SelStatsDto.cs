namespace Core.Entities.Masters
{
    public class SelStatsDto
    {
        public SelStatsDto(int enquiryId, int enquiryItemId, int srNo, string categoryName, int hRExecutiveId, string hRExecName, int selectionCount)
        {
            EnquiryId = enquiryId;
            EnquiryItemId = enquiryItemId;
            SrNo = srNo;
            CategoryName = categoryName;
            HrExecutiveId = hRExecutiveId;
            HrExecName = hRExecName;
            SelectionCount = selectionCount;
        }

        public int EnquiryId {get; set; }
        public int EnquiryItemId {get; set; }
        public int SrNo {get; set; }
        public string CategoryName {get; set; }
        public int HrExecutiveId {get; set; }
        public string HrExecName {get; set;}
        public int SelectionCount {get; set;}
    }
} 