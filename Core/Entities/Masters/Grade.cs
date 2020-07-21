using Core.Enumerations;

namespace Core.Entities.Masters
{
    public class Grade: BaseEntity
    {
        public Grade()
        {
        }

        public int CustomerId { get; set; }
        public enumCustomerGrade CurrentGrade { get; set; }
        public string Remarks { get; set; }
    }
}