using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public static class AccountMapper
    {
        public static AccountModel ToDomain(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new AccountModel
            {
                Id = entity.Id,
                PrimaryContactId = GetGuid(entity, AccountFields.PrimaryContactId),
                CrsNumber = GetString(entity, AccountFields.CrsNumber),
                Name = GetString(entity, AccountFields.Name),
                TradingAs = GetString(entity, AccountFields.TradingAs),
                Telephone = GetString(entity, AccountFields.Telephone),
                Email = GetString(entity, AccountFields.Email),
                CsdNumber = GetString(entity, AccountFields.CsdNumber),
                StatusCode = GetOptionSetValue(entity, AccountFields.StatusCode),
                Grade = GetGuid(entity, AccountFields.Grade),
                Enterprise = GetGuid(entity, AccountFields.Enterprise),
                ProvinceId = GetGuid(entity, AccountFields.ProvinceId),
                EnterpriseRegistrationNumber = GetString(entity, AccountFields.EnterpriseRegistrationNumber),
                AnnualUpdateDueDate = GetDateTime(entity, AccountFields.AnnualUpdateDueDate),
                RenewalDueDate = GetDateTime(entity, AccountFields.RenewalDueDate),
                CurrentContractorGradingDesignation = GetString(entity, AccountFields.CurrentContractorGradingDesignation),
                CurrentContractorGrade = GetString(entity, AccountFields.CurrentContractorGrade),
                DateEnterpriseRegistered = GetDateTime(entity, AccountFields.DateEnterpriseRegistered),
                DateOperationsStarted = GetDateTime(entity, AccountFields.DateOperationsStarted),
                SuspensionReasonAnnualUpdate = GetString(entity, AccountFields.SuspensionReasonAnnualUpdate),
                Type = GetString(entity, AccountFields.Type),
                EnterpriseType = GetString(entity, AccountFields.EnterpriseType)
            };
        }

        public static Entity ToEntity(AccountModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entity = new Entity(CrmEntityNames.Account);

            // Only set Id when updating
            if (model.Id != Guid.Empty)
            {
                entity.Id = model.Id;
                entity[AccountFields.Id] = model.Id;
            }

            if (model.PrimaryContactId.HasValue)
                entity[AccountFields.PrimaryContactId] = new EntityReference("contact", model.PrimaryContactId.Value);

            if (!string.IsNullOrWhiteSpace(model.CrsNumber))
                entity[AccountFields.CrsNumber] = model.CrsNumber;

            if (!string.IsNullOrWhiteSpace(model.Name))
                entity[AccountFields.Name] = model.Name;

            if (!string.IsNullOrWhiteSpace(model.TradingAs))
                entity[AccountFields.TradingAs] = model.TradingAs;

            if (!string.IsNullOrWhiteSpace(model.Telephone))
                entity[AccountFields.Telephone] = model.Telephone;

            if (!string.IsNullOrWhiteSpace(model.Email))
                entity[AccountFields.Email] = model.Email;

            if (!string.IsNullOrWhiteSpace(model.CsdNumber))
                entity[AccountFields.CsdNumber] = model.CsdNumber;

            if (model.StatusCode.HasValue)
                entity[AccountFields.StatusCode] = new OptionSetValue(model.StatusCode.Value);

            if (model.Grade.HasValue)
                entity[AccountFields.Grade] = new EntityReference("nv_grade", model.Grade.Value);

            if (model.Enterprise.HasValue)
                entity[AccountFields.Enterprise] = new EntityReference("nv_enterprisetype", model.Enterprise.Value);

            if (model.ProvinceId.HasValue)
                entity[AccountFields.ProvinceId] = new EntityReference("_nv_provinceid_value", model.ProvinceId.Value);

            if (!string.IsNullOrWhiteSpace(model.EnterpriseRegistrationNumber))
                entity[AccountFields.EnterpriseRegistrationNumber] = model.EnterpriseRegistrationNumber;

            if (model.AnnualUpdateDueDate.HasValue)
                entity[AccountFields.AnnualUpdateDueDate] = model.AnnualUpdateDueDate;

            if (model.RenewalDueDate.HasValue)
                entity[AccountFields.RenewalDueDate] = model.RenewalDueDate;

            if (!string.IsNullOrWhiteSpace(model.CurrentContractorGradingDesignation))
                entity[AccountFields.CurrentContractorGradingDesignation] = model.CurrentContractorGradingDesignation;

            if (!string.IsNullOrWhiteSpace(model.CurrentContractorGrade))
                entity[AccountFields.CurrentContractorGrade] = model.CurrentContractorGrade;

            if (model.DateEnterpriseRegistered.HasValue)
                entity[AccountFields.DateEnterpriseRegistered] = model.DateEnterpriseRegistered;

            if (model.DateOperationsStarted.HasValue)
                entity[AccountFields.DateOperationsStarted] = model.DateOperationsStarted;

            if (!string.IsNullOrWhiteSpace(model.SuspensionReasonAnnualUpdate))
                entity[AccountFields.SuspensionReasonAnnualUpdate] = model.SuspensionReasonAnnualUpdate;

            if (!string.IsNullOrWhiteSpace(model.Type))
                entity[AccountFields.Type] = model.Type;

            if (!string.IsNullOrWhiteSpace(model.EnterpriseType))
                entity[AccountFields.EnterpriseType] = model.EnterpriseType;

            return entity;
        }

        #region Helpers

        private static string? GetString(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<string>(key)
                : null;
        }

        private static Guid? GetGuid(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<Guid>(key)
                : null;
        }

        private static DateTime? GetDateTime(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<DateTime>(key)
                : null;
        }

        private static int? GetOptionSetValue(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<OptionSetValue>(key)?.Value
                : null;
        }

        #endregion
    }
}
