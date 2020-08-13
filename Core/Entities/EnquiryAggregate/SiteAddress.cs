namespace Core.Entities.EnquiryAggregate
{
    public class SiteAddress
    {
        public SiteAddress()
        {
        }

        public SiteAddress(string coName, string gender, string firstName, string lastName, 
            string designation, string mobile, string email, string street, 
            string city, string state, string zipcode)
        {
            CustomerName = coName;
            Gender = gender;
            FirstName = firstName;
            LastName = lastName;
            Designation = designation;
            Mobile = mobile;
            Email = email;
            Street = street;
            City = city;
            State = state;
            Zipcode = zipcode;
        }

        //public int CustomerId {get; set;}
        public string CustomerName {get; set; }
        public string Gender {get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation {get; set; }
        public string Mobile {get; set;}
        public string Email {get; set;}
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
          
    }
}