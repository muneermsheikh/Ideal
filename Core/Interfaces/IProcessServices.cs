using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Processing;
using Core.Enumerations;

namespace Core.Interfaces
{
    public interface IProcessServices
    {
        Task<IReadOnlyList<Process>> AddProcessTransactions(DateTime TransactionDate, 
            enumProcessingStatus status, string remarks, int[] cvrefIds);
        Task<int> UpdateProcessTransactions(List<Process> processings);

        Task<Process> UpdateProcessTransaction (Process process);
        Task<int> DeleteProcessTransaction (Process process);

//GETS
        Task<List<Enquiry>> GetDLProcessDetails(int[] enquiryIds);
        Task<List<EnquiryItem>> GetDLCategoryProcessDetails (int[] enquiryitemIds);
        Task<List<CVRef>> GetCandidateHistory(int candidateId);
    }
}