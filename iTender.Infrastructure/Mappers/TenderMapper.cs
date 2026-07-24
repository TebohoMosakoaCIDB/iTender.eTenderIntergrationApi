using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public static class TenderMapper
    {
        public static TenderModel ToDomain(Entity entity)
        {
            return new TenderModel
            {
                Id = entity.Id,

                EmployerTenderNumber = GetString(entity, TenderFields.EmployerTenderNumber),
                Title = GetString(entity, TenderFields.Title),
                //Name = GetString(entity, TenderFields.Name),

                EmployerId = GetLookupId(entity, TenderFields.EmployerId),
                ProvinceId = GetLookupId(entity, TenderFields.ProvinceId),
                MetroDistrictId = GetLookupId(entity, TenderFields.MetroDistrictId),
                LocalMunicipalityId = GetLookupId(entity, TenderFields.LocalMunicipalityId),

                EmployerName = GetLookupName(entity, TenderFields.EmployerId),
                ProvinceName = GetLookupName(entity, TenderFields.ProvinceId),
                MetroDistrictName = GetLookupName(entity, TenderFields.MetroDistrictId),
                LocalMunicipalityName = GetLookupName(entity, TenderFields.LocalMunicipalityId),

                ClassOfConstructionWorksId = GetLookupId(entity, TenderFields.ClassOfWork),
                AlternateClassOfConstructionWorksName = GetLookupName(entity, TenderFields.AltClassOfWork),
                AlternateClassOfConstructionWorksId = GetLookupId(entity, TenderFields.AltClassOfWork),
                ClassOfConstructionWorksName = GetLookupName(entity, TenderFields.ClassOfWork),
                TypeOfContractId = GetOption(entity, TenderFields.TypeOfContract),

                SubCategoryId = GetLookupId(entity, TenderFields.SubCategory),
                SubCategoryName = GetLookupName(entity, TenderFields.SubCategory),
                AlternateSubCategoryId = GetLookupId(entity, TenderFields.AltSubCategory),
                AlternateSubCategoryName = GetLookupName(entity, TenderFields.AltSubCategory),

                TenderValueRangeId = GetOption(entity, TenderFields.TenderValueRange),
                TenderValueRangeName = GetOptionLabel(entity, TenderFields.TenderValueRange),

                DocumentsAvailableFrom = GetDate(entity, TenderFields.DocsAvailableFrom),
                ClosingDateTime = GetDate(entity, TenderFields.ClosingDate),

                EmergingEnterpriseSupport = GetOption(entity, Contractfields.EmergingEnterpriseSupport),
                PartOfTargetedDevelopmentProgramme = GetOption(entity, Contractfields.ContractorDevelopmentProgramme),
                NameOfTargetedDevelopmentProgramme = GetString(entity, TenderFields.NameOfTargetedDevelopmentProgramme),
                NationalContractorDevelopmentProgramme = GetOption(entity, Contractfields.NationalContractorDevelopmentProgramme),

                //BatchId = GetLookupId(entity, TenderFields.BatchId),

                IsClosed = GetBool(entity, TenderFields.IsClosed) ?? false,

                StatusCodeId = GetOption(entity, TenderFields.StatusCode),

                //IsEmergencyTender = GetBool(entity, TenderFields.IsEmergency),
                IsTermContract = GetBool(entity, TenderFields.IsTerm),
                IsPartOfEPWP = GetBool(entity, TenderFields.EPWP),
                //IsPPP = GetBool(entity, TenderFields.PPP),
                IsEOI = GetBool(entity, TenderFields.IsEOI),
                //IsMunicipalWorks = GetBool(entity, TenderFields.MunicipalWorks),

                DateAdvertised = GetDate(entity, TenderFields.DateAdvertised),
                //NotAcceptedTelephonic = GetBool(entity, "nv_notacceptedtelephonic"),
                //NotAcceptedTelegraphic = GetBool(entity, "nv_notacceptedtelegraphic"),
                //NotAcceptedTelex = GetBool(entity, "nv_notacceptedtelex"),
                //NotAcceptedFacsimile = GetBool(entity, "nv_notacceptedfacsimile"),
                //NotAcceptedEmail = GetBool(entity, "nv_notacceptedemail"),

                //MethodOfPaymentCash = GetBool(entity, "new_methodofpaymentcash"),
                //MethodOfPaymentProofOfDepost = GetBool(entity, "new_methodofpaymentproofofdeposit"),
                //MethodOfPaymentBankGuaranteedCheque = GetBool(entity, "new_methodofpaymentbankguaranteedcheque"),

                ClarificationMeetingRequired = GetOptionLabel(entity, TenderFields.ClarificationMeeting),
                ClarificationMeetingDateAndTime = GetDate(entity, TenderFields.ClarificationMeetingDateAndTime),
                ClarificationMeetingPlace = GetString(entity, TenderFields.ClarificationMeetingPlace),
                ClarificationMeetingCompulsory = GetBool(entity, TenderFields.ClarificationMeetingCompulsory),

                AdditionalClarificationMeetingRequired = GetBool(entity, TenderFields.AdditionalClarificationMeeting),
                AdditionalClarificationMeetingDateAndTime = GetDate(entity, TenderFields.AdditionalClarificationMeetingDateAndTime),
                AdditionalClarificationMeetingPlace = GetString(entity, TenderFields.AdditionalClarificationMeetingPlace),
                AdditionalClarificationMeetingCompulsory = GetBool(entity, TenderFields.AdditionalClarificationMeetingCompulsory),

                TendersInvitedFor = GetString(entity, TenderFields.TendersInvitedFor),
                CidbRefNo = GetString(entity, TenderFields.CidbReferencenNumber),
                PreferencesOffered = GetString(entity, TenderFields.PreferencesOffered),
                EligibilityCriteria = GetString(entity, TenderFields.EligibilityCriteria),

                //ParentTenderId = GetLookupId(entity, TenderFields.ParentTender),

                PrimaryAddressLine1 = GetString(entity, TenderFields.PrimaryAddressLine1),
                PrimaryAddressLine2 = GetString(entity, TenderFields.PrimaryAddressLine2),
                PrimaryAddressLine3 = GetString(entity, TenderFields.PrimaryAddressLine3),
                PrimaryAddressLine4 = GetString(entity, TenderFields.PrimaryAddressLine4),

                PostalAddressLine1 = GetString(entity, TenderFields.SecondaryAddressLine1),
                PostalAddressLine2 = GetString(entity, TenderFields.SecondaryAddressLine2),
                PostalAddressLine3 = GetString(entity, TenderFields.SecondaryAddressLine3),
                PostalAddressLine4 = GetString(entity, TenderFields.SecondaryAddressLine4),

                DepositAmount = GetMoney(entity, TenderFields.DepositAmount),
            };
        }

        private static string GetString(Entity e, string field)
        {
            return e.Contains(field) ? e.GetAttributeValue<string>(field) : null;
        }

        private static Guid? GetLookupId(Entity e, string field)
        {
            return e.GetAttributeValue<EntityReference>(field)?.Id;
        }

        private static int? GetOption(Entity e, string field)
        {
            return e.GetAttributeValue<OptionSetValue>(field)?.Value;
        }

        public static string GetMoney(Entity entity, string field)
        {
            if (!entity.Contains(field)) return "R 0.00";

            var money = entity.GetAttributeValue<Money>(field);

            return money != null
                ? $"R {money.Value:N2}"
                : "R 0.00";
        }

        private static string GetOptionLabel(Entity e, string field)
        {
            return e.FormattedValues.Contains(field)
                ? e.FormattedValues[field]
                : null;
        }

        private static string GetLookupName(Entity e, string field)
        {
            return e.GetAttributeValue<EntityReference>(field)?.Name;
        }

        private static DateTime? GetDate(Entity e, string field)
        {
            return e.GetAttributeValue<DateTime?>(field);
        }

        private static bool? GetBool(Entity e, string field)
        {
            if (!e.Contains(field)) return null;

            var value = e[field];

            if (value is bool b)
                return b;

            if (value is OptionSetValue opt)
                return opt.Value == 1; // adjust if your true value != 1

            return null;
        }
    }
}
