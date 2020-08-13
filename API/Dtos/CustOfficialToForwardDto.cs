namespace API.Dtos
{
    public class CustOfficialToForwardDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name {get; set; }
        public string Gender {get; set;}
        public string email {get; set;}
        public string Mobile {get; set; }  
        public string Mobile2 {get; set; }
    }
}