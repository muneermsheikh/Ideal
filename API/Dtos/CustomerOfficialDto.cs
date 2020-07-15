using System;

namespace API.Dtos
{
    public class CustomerOfficialDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Gender { get; set; } = "M";
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Mobile2 { get; set; }
        public string email { get; set; }
        public string PersonalEmail { get; set; }
        public bool IsValid { get; set; } = true;
        public DateTimeOffset AddedOn { get; set; }
    }
}