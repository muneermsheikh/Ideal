using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumProcessingStatus
    {
        [EnumMember(Value = "Selected")]
        Selected,
        [EnumMember(Value = "Offer Accepted by Candidate")]
        OfferAcceptedByCandidate,
        [EnumMember(Value = "Referred for medical tests")]
        MedicalTestReferred,
        [EnumMember(Value = "Medical Test passed")]
        medicalTestPassed,
        [EnumMember(Value = "Medical Test Failed")]
        medicalTestFailed,
        [EnumMember(Value = "Visa Documents Prepared")]
        VisaDocumentPrepared,
        [EnumMember(Value = "Visa Documents submitted")]
        VisaDocumentsSubmitted,
        [EnumMember(Value = "Visa Received/Endorsed")]
        VisaReceivedEndorsed,
        [EnumMember(Value = "Emigration documents submitted")]
        EmigrationDocumentsSubmitted,
        [EnumMember(Value = "Emigration Cleared")]
        EmigrationCleared,
        [EnumMember(Value = "Emigration Denied")]
        EmigrationDenied,
        [EnumMember(Value = "Travel Booked")]
        TravelBooked,
        [EnumMember(Value = "Traveled")]
        Traveled,
        [EnumMember(Value = "Reached destination")]
        ReachedSite
    }
}