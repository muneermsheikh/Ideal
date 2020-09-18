using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class AssessmentQHeaderDto
    {
        public string CustomerName {get; set;}
        public int EnquiryNo {get; set; }
        public DateTime EnquiryDate {get; set;}
        public int CountOfItems {get; set;}
        public List<AssessmentQDto> ItemAssessmentQs {get; set;}        
    }

    public class AssessmentQForCategoryDto
    {
        public string Category {get; set; }
        public int CountOfItems {get; set; }
        public List<AssessmentQItemDto> AssessmentQItemListDto {get; set;}
    }

    public class AssessmentQDto
    {
        public string CustomerName {get; set;}
        public int EnquiryItemId { get; set; }
        public string CategoryRef {get; set; }
        public string EnquiryNoAndDate {get; set;}
        public int CountOfItems {get; set; }
        public List<AssessmentQItemDto> AssessmentQItemListDto {get; set;}
    }

    public class AssessmentQItemDto
    {
        public AssessmentQItemDto(int questionNo, bool isMandatory, string assessmentParameter, string question, int maxPoints)
        {
            QuestionNo = questionNo;
            IsMandatory = isMandatory;
            AssessmentParameter = assessmentParameter;
            Question = question;
            MaxPoints = maxPoints;
        }

        public int QuestionNo { get; set; }
        public bool IsMandatory {get; set; }
        public string AssessmentParameter { get; set; }
        public string Question { get; set; }
        public int MaxPoints { get; set; }

    }
}