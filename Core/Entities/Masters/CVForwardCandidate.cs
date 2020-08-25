using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Masters
{
    [NotMapped]
    public class CVForwardCandidate
    {
        public CVForwardCandidate(int srNo, int candidateId, int applicationNo, string candidateName,
            string pPNo, string aadharNo, string grade, string photoUrl, string salaryExpectation, 
            string remarks, string countOfCVsForwarded)
        {
            SrNo = srNo;
            CandidateId = candidateId;
            ApplicationNo = applicationNo;
            CandidateName = candidateName;
            PPNo = pPNo;
            AadharNo = aadharNo;
            Grade = grade;
            PhotoUrl = photoUrl;
            SalaryExpectation = salaryExpectation;
            Remarks = remarks;
            CountOfCVsForwarded = countOfCVsForwarded;
        }

        public int SrNo {get; set; }
        public int CandidateId {get; set; }
        public int ApplicationNo {get; set;}
        public string CandidateName {get; set;}
        public string PPNo {get; set;}
        public string AadharNo {get; set;}
        public string Grade {get; set; }
        public string PhotoUrl {get; set; }
        public string SalaryExpectation {get; set; }
        public string Remarks {get; set;}
        public string CountOfCVsForwarded {get; set;}
    }
}