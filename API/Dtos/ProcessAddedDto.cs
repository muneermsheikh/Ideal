namespace API.Dtos
{
    public class ProcessAddedDto
    {
        public ProcessAddedDto()
        {
        }

        public ProcessAddedDto(int cVRefId, string candidateName, string customerName, string categoryRef, string dateOfStatus, string statusInserted, string nextStatusName)
        {
            CVRefId = cVRefId;
            CandidateName = candidateName;
            CustomerName = customerName;
            CategoryRef = categoryRef;
            DateOfStatus = dateOfStatus;
            StatusInserted = statusInserted;
            NextStatusName = nextStatusName;
        }

        public int CVRefId { get; set; }
        public string CandidateName {get; set;}
        public string CustomerName {get; set;}
        public string CategoryRef {get; set;}
        public string DateOfStatus {get; set;}
        public string StatusInserted {get; set;}
        
        public string NextStatusName {get; set;}

    }
}