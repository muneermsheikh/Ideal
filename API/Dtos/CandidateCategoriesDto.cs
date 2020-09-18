using System.Collections.Generic;
using Core.Entities.Admin;

namespace API.Dtos
{
    public class CandidateCategoriesDto
    {
        public string ApplicationNo {get; set; }
        public string CandidateName {get; set;}
        public List<clsString> CategoryNames {get; set; }
    }

}