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
        // 6 to 15 characters, no special characters, alphanumeric characters
        public string Street { get; set; }
        [RegularExpression("(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$",
            ErrorMessage="alphanumeric characters, with min 1 numeric and min 1 alpha, 6 to 15 characters long")]
        public string UserName {get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", 
        ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 characters")]
        public string Password { get; set; }
    }
}