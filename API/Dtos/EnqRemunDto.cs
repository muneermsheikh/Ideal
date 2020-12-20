using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.EnquiryAggregate;

namespace API.Dtos
{
    public class EnqRemunDto
    {
        public EnqRemunDto(int enquiryId, int enquiryNo, DateTime enquiryDate, string customerName, List<Remuneration> lst)
        {
            EnquiryId = enquiryId;
            EnquiryNo = enquiryNo;
            EnquiryDate = enquiryDate;
            CustomerName = customerName;
            Remunerations = lst;
        }

        public int EnquiryId {get; set;}
        public int EnquiryNo {get; set;}
        public DateTime EnquiryDate {get; set;}
        public string CustomerName {get; set;}
        public List<Remuneration> Remunerations {get; set;}
    }

    
}