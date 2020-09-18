using System;
using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace Core.Entities.Dto
{
    public class SelDecisionToAddDto
    {
        public string SelectionRef {get; set;}
        public DateTime SelectionDate {get; set;}
        public enumSelectionResult SelectionResult {get; set; }
        public string Remarks {get; set;}
        public int[] CVRefIds {get; set;}
    }
   
}