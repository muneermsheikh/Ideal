using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace API.Dtos
{
    public class AssessmentToAddDto
    {
        [Required]
        public int EnquiryitemId { get; set; }
        [Required]
        public int CandidateId { get; set; }
        [Required]
        public int AssessedById { get; set; }
        public DateTime AssessedOn { get; set; } = DateTime.Now;
        [Required]
        public List<AssessmentItemToAddDto> AssessmentItemsToAdDto {get; set; }
        public enumAssessmentResult Result { get; set; }=enumAssessmentResult.Referred;
        public string Remarks {get; set; }
    }

}