using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Validations
{
    public class AssessmentValidation
    {
        public string ValidateAssessment(Assessment assessment)
        {
            string errorstring="";

            if(assessment.CandidateId==0) errorstring = "Candidate not defined";
            if(string.IsNullOrEmpty(assessment.CategoryNameAndRef))  errorstring +=  "Category of assessment not defined";
            if(string.IsNullOrEmpty(assessment.CustomerNameAndCity))  errorstring +=  "Cusomer name and city not defined";

            foreach(var q in assessment.AssessmentItems)
            {
                if (q.IsMandatory && q.Assessed !=true) errorstring += "Mmandatory question not assessed";
                if (q.PointsAllotted > q.MaxPoints)  errorstring += "Marks alloted for question No." + q.QuestionNo + " more than maxm points";
                //if (q.PointsAllotted.HasValue & q.Assessed != true) q.Assessed=true;
            }
            return errorstring;
        }
    }
}