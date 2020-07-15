using System.ComponentModel.DataAnnotations;
using Core.Enumerations;

namespace API.Dtos
{
    public class RegisterDto
    {
        public enumCustomerType CustomerType { get; set; } = enumCustomerType.Customer;
        public string CompanyName { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required, MaxLength(50)]
        public string OfficialName { get; set; }
        [Required, MaxLength(1)]
        public string OfficialGender { get; set; }
        public string OfficialDesignation { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string City {get; set; }
        public string Country {get; set; }

        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", 
        ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 characters")]
        public string Password { get; set; }
    }
}