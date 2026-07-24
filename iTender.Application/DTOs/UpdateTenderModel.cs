using iTender.Domain.Enums;
using iTender.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace iTender.Application.DTOs
{
    public class UpdateTenderModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? TendersInvitedFor { get; set; }
        public string? PreferencesOffered { get; set; }
        public string? EligibilityCriteria { get; set; }

        // Location Details
        public Guid? ProvinceId { get; set; }
        public Guid? MetroDistrictId { get; set; }
        public Guid? LocalMunicipalityId { get; set; }

        // Contract Details
        public int? TypeOfContractId { get; set; }
        public Guid? ClassOfConstructionWorksId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public Guid? AlternateClassOfConstructionWorksId { get; set; }
        public int? TenderValueRangeId { get; set; }
        public int? EmergingEnterpriseSupportId { get; set; }
        public string? NameOfTargetedDevelopmentProgramme { get; set; }
        public int? NationalContractorDevelopmentProgrammeId { get; set; }
        public int? IsTermContract { get; set; }
        public TenderStatus? Status { get; set; }

        // Employer Details
        public AddressModel? PrimaryAddress { get; set; } = new();
        public AddressModel? AdditionalCollectionAddress { get; set; } = new();
        public DateTime? DocumentsAvailableFrom { get; set; }
        public decimal? DepositAmount { get; set; }
        public bool? MethodOfPaymentBankGuaranteedCheque { get; set; }
        public bool? MethodOfPaymentCash { get; set; }
        public bool? MethodOfPaymentProofOfDepost { get; set; }
        public string? FurtherPaymentAndCollectionInformation { get; set; }

        // Contact Details
        public List<ContactForTenderModel>? ContactPerson { get; set; } = [];

        // Clarification Meeting Details
        public int? ClarificationMeetingRequired { get; set; }
        public string? ClarificationMeetingPlace { get; set; }
        public DateTime? ClarificationMeetingDateAndTime { get; set; }
        public int? ClarificationMeetingCompulsory { get; set; }
        public int? AdditionalClarificationMeeting { get; set; }
        public string? AddClarificationMeetingPlace { get; set; }
        public DateTime? AddClarificationMeetingDateAndTime { get; set; }
        public int? AddClarificationMeetingCompulsory { get; set; }

        // Submission Details
        public DateTime? ClosingDateTime { get; set; }
        public bool? NotAcceptedEmail { get; set; }
    }
}
