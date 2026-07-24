using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public class ContractMapper
    {
        public static ContractModel ToDomain(Entity e)
        {
            if (e == null) return null;

            var model = new ContractModel
            {
                ContractId = e.Id,

                ContractNumber = e.GetAttributeValue<string>(Contractfields.ContractNumber),
                ContractTitle = e.GetAttributeValue<string>(Contractfields.ContractTitle),
                ContractDescription = e.GetAttributeValue<string>(Contractfields.ContractDescription),

                CidbContractNumber = e.GetAttributeValue<string>(Contractfields.CIDBContractNumber),

                EmployerId = e.GetAttributeValue<EntityReference>(Contractfields.Employer_contract)?.Id ?? Guid.Empty,
                EmployerName = e.GetAttributeValue<EntityReference>(Contractfields.Employer_contract)?.Name,

                TenderId = e.GetAttributeValue<EntityReference>(Contractfields.TenderId)?.Id ?? Guid.Empty,

                ProvinceId = e.GetAttributeValue<EntityReference>(Contractfields.ProvinceID)?.Id ?? Guid.Empty,
                ProvinceName = e.GetAttributeValue<EntityReference>(Contractfields.ProvinceID)?.Name,
                MetroDistrictMunicipalityId = e.GetAttributeValue<EntityReference>(Contractfields.MetroDistrictMunicipalityID)?.Id ?? Guid.Empty,
                LocalMunicipalityId = e.GetAttributeValue<EntityReference>(Contractfields.MunicipalityId)?.Id ?? Guid.Empty,

                ClassOfConstructionWorkId = e.GetAttributeValue<EntityReference>(Contractfields.ClassOfConstructionWorks)?.Id ?? Guid.Empty,

                TypeOfContractId = e.GetAttributeValue<OptionSetValue>(Contractfields.TypeOfContract)?.Value ?? 0,
                //TypeOfContractName = e.GetAttributeValue<OptionSetValue>(Contractfields.TypeOfContract)?.Name ?? 0,
                TenderValueRangeId = e.GetAttributeValue<OptionSetValue>(Contractfields.TenderValueRage)?.Value ?? 0,

                DateAward = e.GetAttributeValue<DateTime?>(Contractfields.ContractAwardDate)?.ToString("dd/MM/yyyy"),

                EmergingEnterpriseSupportId = e.GetAttributeValue<OptionSetValue>(Contractfields.EmergingEnterpriseSupport)?.Value ?? 0,
                FinancialManagementId = e.GetAttributeValue<OptionSetValue>(Contractfields.FinancialManagement)?.Value ?? 0,
                OtherFinancialManagement = e.GetAttributeValue<string>(Contractfields.FinancialManagementOtherDescription),

                ExpandedPublicWorksProgrammeId = e.GetAttributeValue<OptionSetValue>(Contractfields.ExpandedPublicWorksProgramme)?.Value ?? 0,
                ContractorDevelopmentProgrammeId = e.GetAttributeValue<EntityReference>(Contractfields.ContractorDevelopmentProgramme)?.Id ?? Guid.Empty,
                NationalContractorDevelopmentProgrammeId = e.GetAttributeValue<OptionSetValue>(Contractfields.NationalContractorDevelopmentProgramme)?.Value ?? 0,

                PublicPrivatePartnershipId = e.GetAttributeValue<OptionSetValue>(Contractfields.PublicPrivatePartnership)?.Value ?? 0,

                YearId = e.GetAttributeValue<OptionSetValue>(Contractfields.Years)?.Value ?? 0,
                MonthId = e.GetAttributeValue<OptionSetValue>(Contractfields.Months)?.Value ?? 0,
                DayId = e.GetAttributeValue<OptionSetValue>(Contractfields.Days)?.Value ?? 0,

                IsTermContractId = e.GetAttributeValue<OptionSetValue>(Contractfields.IsTermContract)?.Value ?? 0,

                IsJV = e.GetAttributeValue<OptionSetValue>(Contractfields.IsJV)?.Value ?? 0,
                NumberOfContractorsId = e.GetAttributeValue<OptionSetValue>(Contractfields.NumberOfContractors)?.Value ?? 0,

                AverageAnnualContractId = e.GetAttributeValue<OptionSetValue>(Contractfields.AverageAnnualContract)?.Value ?? 0,

                ChangeRequestStatusId = e.GetAttributeValue<OptionSetValue>(Contractfields.ChangeRequestStatus)?.Value ?? 0,

                ContractEndDate = e.GetAttributeValue<DateTime?>(Contractfields.ContractEndDate)?.ToString("dd/MM/yyyy"),
                PracticalCompletionDate = e.GetAttributeValue<DateTime?>(Contractfields.DateOfPracticalCompletion)?.ToString("dd/MM/yyyy"),

                CreatedDate = e.GetAttributeValue<DateTime?>("createdon")?.ToString("dd/MM/yyyy"),

                ContractStatus = e.FormattedValues.Contains("statuscode")
                    ? e.FormattedValues["statuscode"]
                    : null,

                ChangeRequestStatus = e.FormattedValues.Contains(Contractfields.ChangeRequestStatus)
                    ? e.FormattedValues[Contractfields.ChangeRequestStatus]
                    : null,

                Grade = e.GetAttributeValue<string>(Contractfields.Grade),
                ContractValue = e.GetAttributeValue<Money>(Contractfields.TotalContractValueIncludingVat)?.Value ?? 0,
                ContractInvoicedAmount = e.GetAttributeValue<Money>(Contractfields.ContractTotalInvoicedAmountInclVat)?.Value ?? 0,
                ContractPeriodString = e.GetAttributeValue<string>(Contractfields.ContractPeriod),

                TypeOfContractName =
                    e.FormattedValues.Contains(Contractfields.TypeOfContract)
                        ? e.FormattedValues[Contractfields.TypeOfContract]
                        : null,

                ContractPeriodYears = GetIntValue(e, Contractfields.ContractPeriodYears),

                ContractPeriodMonths = GetIntValue(e, Contractfields.ContractPeriodMonths),

                ContractPeriodDays = GetIntValue(e, Contractfields.ContractPeriodDays),

                FinancialPointsTotal = GetIntValue(e, Contractfields.FinancialPointsTotal),

                PreferencePointsTotal = GetIntValue(e, Contractfields.PreferencePointsTotal),

                isPublicPrivatePartnership =
                    e.FormattedValues.Contains(Contractfields.PublicPrivatePartnership)
                        ? e.FormattedValues[Contractfields.PublicPrivatePartnership]
                        : null,

                isExpandedPublicWorksProgramme =
                    e.FormattedValues.Contains(Contractfields.ExpandedPublicWorksProgramme)
                        ? e.FormattedValues[Contractfields.ExpandedPublicWorksProgramme]
                        : null,

                PreferencePointsOutOfId = e.GetAttributeValue<OptionSetValue>(Contractfields.PreferencePoints).Value,
                FinancialPointsOutOfId = e.GetAttributeValue<OptionSetValue>(Contractfields.FinancialPoints).Value,
            };


            return model;
        }

        private static int GetIntValue(Entity e, string fieldName)
        {
            if (!e.Contains(fieldName) || e[fieldName] == null)
                return 0;

            return e[fieldName] switch
            {
                int i => i,
                OptionSetValue o => o.Value,
                _ => 0
            };
        }

        public static Entity ToEntity(ContractModel model)
        {
            var e = new Entity(CrmEntityNames.Contract);

            if (model.ContractId != Guid.Empty)
                e.Id = model.ContractId;

            e[Contractfields.ContractNumber] = model.ContractNumber;
            e[Contractfields.ContractTitle] = model.ContractTitle;
            e[Contractfields.ContractDescription] = model.ContractDescription;

            e[Contractfields.CIDBContractNumber] = model.CidbContractNumber;

            if (model.EmployerId != Guid.Empty)
                e[Contractfields.Employer_contract] = new EntityReference("account", model.EmployerId);

            if (model.TenderId != Guid.Empty)
                e[Contractfields.TenderId] = new EntityReference("nv_tender", model.TenderId);

            if (model.ProvinceId != Guid.Empty)
                e[Contractfields.ProvinceID] = new EntityReference("nv_province", model.ProvinceId);

            if (model.MetroDistrictMunicipalityId != Guid.Empty)
                e[Contractfields.MetroDistrictMunicipalityID] = new EntityReference("nv_metrodistrict", model.MetroDistrictMunicipalityId);

            if (model.LocalMunicipalityId != Guid.Empty)
                e[Contractfields.MunicipalityId] = new EntityReference("nv_municipality", model.LocalMunicipalityId);

            if (model.ClassOfConstructionWorkId != Guid.Empty)
                e[Contractfields.ClassOfConstructionWorks] = new EntityReference("nv_classofconstructionworks", model.ClassOfConstructionWorkId);

            e[Contractfields.TypeOfContract] = new OptionSetValue(model.TypeOfContractId);
            e[Contractfields.TenderValueRage] = new OptionSetValue(model.TenderValueRangeId);

            if (!string.IsNullOrWhiteSpace(model.DateAward))
                e[Contractfields.ContractAwardDate] = DateTime.Parse(model.DateAward);

            e[Contractfields.EmergingEnterpriseSupport] = new OptionSetValue(model.EmergingEnterpriseSupportId);
            e[Contractfields.FinancialManagement] = new OptionSetValue(model.FinancialManagementId);
            e[Contractfields.FinancialManagementOtherDescription] = model.OtherFinancialManagement;

            e[Contractfields.ExpandedPublicWorksProgramme] = new OptionSetValue(model.ExpandedPublicWorksProgrammeId);

            if (model.ContractorDevelopmentProgrammeId != Guid.Empty)
                e[Contractfields.ContractorDevelopmentProgramme] = new EntityReference("nv_contractordevelopmentprogramme", model.ContractorDevelopmentProgrammeId);

            e[Contractfields.NationalContractorDevelopmentProgramme] = new OptionSetValue(model.NationalContractorDevelopmentProgrammeId);

            e[Contractfields.PublicPrivatePartnership] = new OptionSetValue(model.PublicPrivatePartnershipId);

            e[Contractfields.Years] = model.YearId;
            e[Contractfields.Months] = model.MonthId;
            e[Contractfields.Days] = model.DayId;

            e[Contractfields.Grade] = model.Grade;

            e[Contractfields.IsTermContract] = new OptionSetValue(model.IsTermContractId);

            e[Contractfields.IsJV] = new OptionSetValue(model.IsJV);
            e[Contractfields.NumberOfContractors] = new OptionSetValue(model.NumberOfContractorsId);

            e[Contractfields.AverageAnnualContract] = new OptionSetValue(model.AverageAnnualContractId);

            if (!string.IsNullOrWhiteSpace(model.ContractEndDate))
                e[Contractfields.ContractEndDate] = DateTime.Parse(model.ContractEndDate);

            return e;
        }
    }
}
