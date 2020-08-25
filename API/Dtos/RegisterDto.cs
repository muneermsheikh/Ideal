using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace API.Dtos
{
     public class RegisterDto
    {
        //public CustomerAddress customerAddress{get; set; }
        
        public enumCustomerType CustomerType { get; set; } = enumCustomerType.Candidate;
        public int CustomerId { get; set; }
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public int EmployeeId {get; set; }
        public string CompanyName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required, MaxLength(50)]
        public string FirstName {get; set; }
        public string FamilyName {get; set; }
        [Required, MaxLength(1)]
        public string Gender { get; set; }
        public string Designation { get; set; }
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Mobile {get; set; }
        public string IntroducedBy {get; set; }
        public string Street { get; set; }
        public string UserName {get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", 
        ErrorMessage = "Password should be 6-15 char long, min 1 char numeric, min 1 char uppercase, min 1 char lower case, min 1 numeric char and 1 special char")]
        public string Password { get; set; }
    }
}