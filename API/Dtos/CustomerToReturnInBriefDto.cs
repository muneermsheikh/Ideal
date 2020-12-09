namespace API.Dtos
{
    public class CustomerToReturnInBriefDto
    {
        public CustomerToReturnInBriefDto(int id, string customerName, string knownAs, string cityName) 
        {
            this.Id = id;
            this.CustomerName = customerName;
            this.KnownAs = knownAs;
            this.CityName = cityName;
        }
        
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string KnownAs { get; set; }
        public string CityName { get; set; }

    }
}