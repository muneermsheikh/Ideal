using System;

namespace Core.Entities.Processing
{
    public class Travel: BaseEntity
    {
        public Travel()
        {
        }

        public Travel(int processId, int cVRefId, string bordingAirport, string destinationAirport, string airline, string flightNo, DateTime eTD, DateTime eTA, DateTime bookedOn, string pNR)
        {
            ProcessId = processId;
            CVRefId = cVRefId;
            BoardingAirport = bordingAirport;
            DestinationAirport = destinationAirport;
            Airline = airline;
            FlightNo = flightNo;
            ETD = eTD;
            ETA = eTA;
            BookedOn = bookedOn;
            PNR = pNR;
        }

        public int ProcessId {get; set;}
        public int CVRefId {get; set;}
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