namespace API.Dtos
{
    public class EnquiryItemInBriefDto
    {
        public int Id {get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool ECNR { get; set; }
    }
}