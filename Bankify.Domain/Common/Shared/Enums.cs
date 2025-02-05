using System.ComponentModel;
namespace Bankify.Domain.Models.Shared
{
    public enum UserStatusAction
    {
        [Description("Mark As Active")]
        MarkAsActive = 1,
        [Description("Mark As In Active")]
        MarkAsInActive = 2
    }

    public enum RecordStatus
    {
        InActive = -1,
        Active = 1,
        Deleted = 0,
        Closed = 2
    }
    public enum ContractStatus
    {
        Inactive = -1,
        Draft = 0,
        Active = 1,
        Pending = 2,
        Terminated = 3,
        Archived = 4,
        In_Negotation = 5,
        Expired = 6,
        Reversed = 7,
        Superseded = 8,
    }

    public enum ReminderFrequency
    {
        OneOff = 1,
        Daily = 2,
        Every3Days = 3,
        OnceAweek = 4,
        Every2Weeks = 5,
        EveryMonth = 6,
    }
    public enum EndOfTermBehaviour
    {
        NoAction = 0,
        Expired = -1,
        Prolong = 1,

    }
    public enum ReminderDeliveryStatus
    {
        New = 0,
        Unsuccessful = -1,
        Delivered = 1,
        RenewDuetoUpdate = 2,
        Escaleted = 3,
    }

    public enum Region
    {
        Local = 1,
        International = 2,
    }
    public enum ContractFilter
    {
        Next12MonthEvents,
        OverdueEvents,
        Next12MonthReminders,
        Expired,
        Within1Month,
        Within1To3Months,
        Within3To6Months,
        Within6To12Months,
        Archived,
        ActiveContracts,
        AllContracts
    }
    public enum ReminderFilter
    {
        Expired = 0,
        Active = 1,
    }

    public enum LifeCycle
    {
        FixedPeriodWithRenewal = 1,
        FixedPeroidWithoutRenewal = 2,
        OpenEnded = 3,
    }
    public enum EntityType
    {
        Contract = 1,
        Task = 2,
        EventDesktop = 3,
        EventEmail = 4,
    }
    public enum EmailType
    {
        UserEmail = 1,
        CcEmail = 2,
    }
    public enum PeopleGroup1
    {
        Client = 1,
        Co_Counsel = 2,
        Expert = 3,
        Judge = 4,
        Un_assigned = 5
    }
    public enum Gender
    {
        Male = 1,
        Female = 2,
    }
    public enum CaseStage
    {
        Discovery = 1,
        In_Trial = 2,
        On_Hold = 3,
    }
    public enum ActivityLog
    {
        Status = 1, // If there is change in case status
        Task = 2, // If case task created/changed
        Event = 3, // If case event created/changed
        Note = 4, // If case note added/updated
        Stage = 5, // If case stage updated
        CaseInfo = 6, // If there is change in case fields
    }
    public enum Priority
    {
        No_Priority = 0,
        Low = 1,
        Medium = 2,
        High = 3,
    }
    public enum TaskCompletion
    {
        Pending = 0,
        Completed = 1,
        In_Progress = 2,
    }
    public enum CaseStatus
    {
        Open = 1,
        Closed = 2,
    }
    public enum AttorneyType
    {
        LeadAttorney = 1,
        OrginatingAttorney = 2
    }
    public enum CaseParty
    {
        Defendant = 1,
        Plaintiff = 2,
        Intervening = 3,
        Applicant = 4,
        Respondent = 5,
        Opposing_Party = 6

    }
    public enum CounselType
    {
        Individual = 1,
        Company = 2,
    }
    public enum ClientType
    {
        Individual = 1,
        Company = 2,
    }
    public enum CaseLifecycleStatus
    {
        Opened = 1,
        Closed = 2,
        ReOpened = 3,
    }
    public enum CollaborationType
    {
        contract_review = 1,
        case_claims_review = 2,
        regulatory_compliance_issues = 3,
        administrative_matters = 4,
        templates = 5,
    }
    public enum ManagementStatus
    {
        Management=1,
        Non_Management=2,
    }
    public enum TaskType
    {
        cases=1,
        contracts=2,
        matters=3,
        document_review=4,
        others=5,
        
    }
}
