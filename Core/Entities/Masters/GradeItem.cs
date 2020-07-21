using System;

namespace Core.Entities.Masters
{
    public class GradeItem: BaseEntity
    {
        public GradeItem()
        {
        }

        public int GradeId { get; set; }
        public DateTime GradedOn { get; set; }
        public Employee GradedBy { get; set; }
        public string Remarks { get; set; }
    }
}