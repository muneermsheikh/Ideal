using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class AddressDto
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string AppUserId { get; set; }
        public int EmployeeId {get; set; }
        public int CustomerId {get; set; }
        public string CompanyName {get; set; }
        public string OfficialGender {get; set; }
        public string OfficialName {get; set; }
        public string OfficialDesignation {get; set; }
        public string Country {get; set;}

   }
}