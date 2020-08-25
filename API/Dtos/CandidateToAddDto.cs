using System;
using System.Collections.Generic;
using Core.Entities.HR;
using Core.Enumerations;

namespace API.Dtos
{
    public class CandidateToAddDto
    {
         public int ApplicationNo { get; set; }
        public DateTime ApplicationDated { get; set; }
        
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string PPNo { get; set; }
        public string AadharNo { get; set; }
        public DateTime DOB { get; set; }

        public virtual CandidateAddress CandidateAddress { get; set; }
        public enumCandidateStatus CandidateStatus { get; set; } = enumCandidateStatus.Available;
        public virtual List<Attachment> Attachments {get; set; }

    }
}