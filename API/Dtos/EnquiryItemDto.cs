using System;

namespace API.Dtos
{
    public class EnquiryItemDto
    {
        public int CategoryItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool ECNR { get; set; }
/*        public int ExpDesiredInYrsMin { get; set; }
        public int ExpDesiredInYrsMax { get; set; }
        public string JobDescInBrief { get; set; }
        
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }

        public int ContractPeriodInMonths { get; set; }
        public string Food { get; set; }
        public string Housing { get; set; }
        public string Transport {get; set;}
*/
        public DateTime CompleteBy { get; set; }
        public string Status { get; set; }
        public string AssessingHRExec {get; set;}
        public string AssessingHRSup {get; set;}
        public string AssessingHRM {get; set; }
    }
}