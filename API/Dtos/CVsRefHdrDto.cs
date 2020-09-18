using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace API.Dtos
{
    public class CVRefHdrDto
    {
        public DateTime DateReferred {get; set; }
        public string customername {get; set; }
        public string officialname {get; set;}
        public string officialemail {get; set;}
        public string customercity {get; set;}
        public int countofcvs {get; set; }
        public List<CVRefItemDto> CVRefItemsDto {get; set;}
    }
    
    public class CVRefItemDto
    {
        public string categoryref {get; set;}
        public int srno {get; set;}
        public int applicationno {get; set;}
        public string candidatename {get; set;}
        public string ppno {get; set; }
        public string photourl {get; set;}
        public string salaryexpectation {get; set; }
        public string grade {get; set;}
    }

    public class cvrefenquiryno: IComparable<cvrefenquiryno>
    {
        public int enquiryitemid {get; set;}
        public int candidateid {get; set;}

        public int CompareTo([AllowNull] cvrefenquiryno c)
        {
            return this.enquiryitemid.CompareTo(c.enquiryitemid);
        }
    }
}