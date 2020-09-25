using System;

namespace Core.Entities
{
    public class TravelToAddDto
    {
        public string BoardingAirport {get; set;}
        public string DestinationAirport {get; set;}
        public string Airline {get; set;}
        public string FlightNo {get; set;}
        public DateTime ETD {get; set;}
        public DateTime ETA {get; set;}
        public DateTime BookedOn {get; set;}
        public string PNR {get; set;}
        
    }
}