using System;

namespace API.Dtos
{
    public class CustomerOfficialDto
    {
        public int Id {get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        public string email { get; set; }
        public string PersonalEmail { get; set; }
        public bool IsValid { get; set; } 
        public DateTime AddedOn { get; set; }
    }
}