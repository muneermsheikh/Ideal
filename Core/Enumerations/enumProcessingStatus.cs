using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumProcessingStatus
    {
        [EnumMember(Value = "Selected")]
        Selected=100,

        [EnumMember(Value = "Offer Accepted by Candidate")]
        OfferAcceptedByCandidate=500,

        [EnumMember(Value = "Referred for medical tests")]
        MedicalTestReferred=1000,

        [EnumMember(Value = "Medical Test passed")]
        medicalTestPassed=2000,

        [EnumMember(Value = "Medical Test Failed")]
        medicalTestFailed=2500,

        [EnumMember(Value = "Visa Documents Prepared")]
        VisaDocumentPrepared=3000,

        [EnumMember(Value = "Visa Documents submitted")]
        VisaDocumentsSubmitted=4000,

        [EnumMember(Value = "Visa Received/Endorsed")]
        VisaReceivedEndorsed=5000,

        [EnumMember(Value = "Emigration documents submitted")]
        EmigrationDocumentsSubmitted=6000,

        [EnumMember(Value = "Emigration Cleared")]
        EmigrationCleared=7000,

        [EnumMember(Value = "Emigration Denied")]
        EmigrationDenied=7500,

        [EnumMember(Value = "Travel Booked")]
        TravelBooked=8000,

        [EnumMember(Value = "Traveled")]
        Traveled=9000,
        
        [EnumMember(Value = "Reached destination")]
        ReachedSite=10000
    }
}