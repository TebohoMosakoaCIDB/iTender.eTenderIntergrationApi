using iTender.Application.DTOs;
using iTender.Domain.Models;

namespace iTender.Infrastructure.Mappers
{
    public static class ExternalTenderMapper
    {
        public static ExternalTenderModel ToExternal(this TenderModel tender)
        {
            var contact = tender.ContactPerson?.FirstOrDefault();

            return new ExternalTenderModel
            {
                // External expects an int. Since your Id is Guid, use 0 or another mapping.
                Id = 0,

                Tender_No = tender.EmployerTenderNumber,
                Type = tender.TypeOfContractName,

                Delivery = string.Join(", ",
                    new[]
                    {
                    tender.PrimaryAddressLine1,
                    tender.PrimaryAddressLine2,
                    tender.PrimaryAddressLine3,
                    tender.PrimaryAddressLine4
                    }.Where(x => !string.IsNullOrWhiteSpace(x))),

                Department = tender.EmployerName,

                Date_Published = tender.DateAdvertised,

                Cbrief = tender.ClarificationMeetingRequired?.Equals("Yes", StringComparison.OrdinalIgnoreCase),

                Cd = tender.ClosingDateTime?.ToString("dddd, dd MMMM yyyy - HH:mm"),

                Dp = tender.DateAdvertised?.ToString("dddd, dd MMMM yyyy"),

                Closing_Date = tender.ClosingDateTime,

                Brief = tender.ClarificationMeetingDateAndTime?.ToString("dddd, dd MMMM yyyy - HH:mm"),

                Compulsory_Briefing_Session = tender.ClarificationMeetingDateAndTime,

                DepartmentId = tender.EmployerId,

                ProvinceId = tender.ProvinceId,

                Status = tender.IsClosed ? "Closed" : "Published",

                Category = tender.SubCategoryName,

                Description = tender.Title,

                Province = tender.ProvinceName,

                ContactPerson = contact?.PersonToQuery,

                Email = contact?.Email,

                Telephone = contact?.TelephoneNumber,

                Fax = contact?.FaxNumber,

                BriefingVenue = tender.ClarificationMeetingPlace,

                Conditions = tender.EligibilityCriteria,

                SupportDocuments = null,

                Bf = tender.ClarificationMeetingRequired,

                BriefingSession = null,

                Bc = tender.ClarificationMeetingCompulsory == true ? "Yes" : "No",

                BriefingCompulsory = tender.ClarificationMeetingCompulsory,

                ESubmission = false,

                ClosingReason = null,

                CancelledReason = null,

                AwardedCompanies = null,

                Bidders = null,

                Awarded_Contact = null,

                CancellationReason = null
            };
        }

        public static TenderModel ToInternal(this ExternalTenderModel tender)
        {
            var model = new TenderModel
            {
                EmployerTenderNumber = tender.Tender_No,
                Title = tender.Description,
                EmployerName = tender.Department,
                ProvinceName = tender.Province,
                SubCategoryName = tender.Category,
                TypeOfContractName = tender.Type,
                DateAdvertised = tender.Date_Published,
                ClosingDateTime = tender.Closing_Date,
                ClarificationMeetingDateAndTime = tender.Compulsory_Briefing_Session,
                ClarificationMeetingPlace = tender.BriefingVenue,
                EligibilityCriteria = tender.Conditions,
                IsClosed = string.Equals(
                    tender.Status,
                    "Closed",
                    StringComparison.OrdinalIgnoreCase)
            };

            if (!string.IsNullOrWhiteSpace(tender.ContactPerson))
            {
                model.ContactPerson.Add(new ContactForTenderModel
                {
                    PersonToQuery = tender.ContactPerson,
                    TelephoneNumber = tender.Telephone,
                    Email = tender.Email
                });
            }

            return model;
        }
    }
}
