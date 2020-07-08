using System.Collections.Generic;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Masters;

namespace Core.Entities.Admin
{
    public class CVSource: BaseEntity
    {
        public int ApplicationId { get; set; }
        public int ApplicationNo { get; set; }
        public List<EnquiryItem> EnquiryItems { get; set; }     // CV is suitable for one or more Requirements - EnquiryItem
        public virtual Customer ReceivedFromAssociate { get; set; }
        public virtual Candidate ReceivedFromCandidate { get; set; }
        public virtual Source Source { get; set; }        //source of CV
    }
}