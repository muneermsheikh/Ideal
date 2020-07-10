namespace Core.Entities.EnquiryAggregate
{
    // this will be part of (owned by) Enquiry table
    public class CategoryItemOrdered
    {
        public CategoryItemOrdered()
        {
        }

        public CategoryItemOrdered(int categoryItemId, string categoryName)
        {
            CategoryItemId = categoryItemId;
            CategoryName = categoryName;
        }

        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
     }
}