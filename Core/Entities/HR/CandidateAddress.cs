namespace Core.Entities.HR
{
  // TO DO -  Consider default value for Country depending upon the country logged-in from
    public class CandidateAddress
    {
        public CandidateAddress()
        {
            Country = "India";
        }

        public int CandidateId { get; set; }
        public string AddressType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public bool Valid { get; set; }
    }
}