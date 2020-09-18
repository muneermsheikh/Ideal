using System;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;

namespace Core.Entities.Admin
{
    public class Emolument: BaseEntity
    {
        public Emolument()
        {
        }

        public Emolument(int cvRefID, string salaryCurrency, int basicSalary, int weeklyWorkHours, 
            int contractPeriodInMonths, enumProvision housing, int housingAllowance, enumProvision food, 
            int foodAllowance, enumProvision transport, int transportAllowance, enumProvision otherAllowance, 
            int otherAllowanceAmount, string airportOfBoarding, string airportOfDestination, 
            bool offerAcceptedByCandidate, DateTime offerAcceptedOn)
        {
            CVRefId = cvRefID;
            SalaryCurrency = salaryCurrency;
            BasicSalary = basicSalary;
            WeeklyWorkHours = weeklyWorkHours;
            ContractPeriodInMonths = contractPeriodInMonths;
            Housing = housing;
            HousingAllowance = housingAllowance;
            Food = food;
            FoodAllowance = foodAllowance;
            Transport = transport;
            TransportAllowance = transportAllowance;
            OtherAllowance = otherAllowance;
            OtherAllowanceAmount = otherAllowanceAmount;
            AirportOfBoarding = airportOfBoarding;
            AirportOfDestination = airportOfDestination;
            OfferAcceptedByCandidate = offerAcceptedByCandidate;
            OfferAcceptedOn = offerAcceptedOn;
        }

        public int CVRefId {get; set;}
        public string SalaryCurrency {get; set;}
        public int BasicSalary {get; set;}
        public int WeeklyWorkHours {get; set;} 
        public int ContractPeriodInMonths{get; set; }
        public enumProvision Housing {get; set;}
        public int HousingAllowance {get; set;}
        public enumProvision Food {get; set;}
        public int FoodAllowance {get; set; }=0;
        public enumProvision Transport {get; set; }
        public int TransportAllowance {get; set;}
        public enumProvision OtherAllowance {get; set;} 
        public int OtherAllowanceAmount {get; set; }
        public int LeaveEntitlementAfterMonths {get; set;}
        public int AnnualLeaveInDays {get; set;}
        public string AirportOfBoarding {get; set;}
        public string AirportOfDestination {get; set;}
        public bool OfferAcceptedByCandidate {get; set;}
        public DateTime OfferAcceptedOn {get; set;}
        public bool OfferRevised {get; set; }=false;
        public string OfferLetterUrl {get; set;}
    }

    
}