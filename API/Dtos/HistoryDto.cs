using System.Collections.Generic;
using Core.Entities.Processing;

namespace API.Dto
{
    
    public class HistoryDto
    {
        public string CandidateName {get; set;}
        public List<HistoryReferred> ReferredList {get; set; }
    }

    public class HistoryReferred
    {
        public HistoryReferred(string categoryRef, string companyName, string dateReferred, List<HistoryProcess> HistList)
        {
            CategoryRef = categoryRef;
            CompanyName = companyName;
            DateReferred = dateReferred;
            HistoryProcesses = HistList;
        }

        public string CategoryRef {get; set;}
        public string CompanyName {get; set; }
        public string DateReferred {get; set;}
        public List<HistoryProcess> HistoryProcesses {get; set; }
    }

    public class HistoryProcess
    {
        public HistoryProcess(string transactionDate, string statusName, string nextStatusName)
        {
            TransactionDate = transactionDate;
            StatusName = statusName;
            NextStatusName = nextStatusName;

        }

        public string TransactionDate {get; set;}
        public string StatusName {get; set;}
        public string NextStatusName {get; set;}
    }
}