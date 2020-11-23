using Core.Entities.Admin;

namespace Core.Entities.Masters
{
    public class CustomerIndustryType: BaseEntity
    {
        public int CustomerId {get; set;}
        public int IndustryTypeId {get; set; }
        public string Name {get; set;}

        public virtual Customer Customer {get; set;}
    }
}