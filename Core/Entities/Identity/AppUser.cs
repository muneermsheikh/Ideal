using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName {get; set; }
        public string CompanyName {get; set; }
        public string OfficialGender {get; set; }
        public string OfficialName {get; set; }
        public string OfficialDesignation {get; set; }
        public Address Address {get; set; }
        public string City {get; set; }
        public string Country {get; set;}

    }
}