using System.Runtime.Serialization;

namespace Core.Enumerations
{
    public enum enumTaskType
    {
        [EnumMember(Value = "Administrative")]
        Administrative,

        [EnumMember(Value = "HR Executive Assignment")]
        HRExecutiveAssignment,

        [EnumMember(Value = "HR Supervisor Assignment")]
        HRSupervisorAssignment,

        [EnumMember(Value = "HR Department Head Assignment")]
        HRDeptHeadAssignment,

        [EnumMember(Value = "Administrative Document Controller Assignment")]
        AdminDocumentControllerAssignment,

        [EnumMember(Value = "Processing Document Controller Assignment")]
        ProcessingDocumentControllerAssignment,

        [EnumMember(Value = "Medical Test Assignment")]
        MedicalTestsAssignment,

        [EnumMember(Value = "Visa Documentation Assignment")]
        VisaDocumentationAssignment,

        [EnumMember(Value = "Ticketing Assignment")]
        TicketingAssignment,
        [EnumMember(Value = "CV Acknowledgement to candidate")]
        CVAcknowledgementToCandidate


    }
}