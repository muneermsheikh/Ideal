using System;
using Core.Enumerations;

namespace API.Dtos
{
    public class ProcessTransactionsAddDto
    {
        public DateTime TransactionDate {get; set;}

        public enumProcessingStatus ProcessingStatus {get; set;}
        public string Remarks {get; set;}
        public int[] CVRefIds {get; set;}
    }
}