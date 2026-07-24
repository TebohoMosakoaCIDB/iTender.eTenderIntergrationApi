using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public static class ApplicationMapper
    {
        public static ApplicationModel ToDomain(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new ApplicationModel
            {
                Id = entity.Id,

                ApplicationNumber = GetString(entity, ApplicationFields.ApplicationNumber),
                Type = GetString(entity, ApplicationFields.Type),

                StatusCode = GetOptionSet(entity, ApplicationFields.Status),
                StateCode = GetOptionSet(entity, ApplicationFields.StateCode),

                ContractorId = GetGuid(entity, ApplicationFields.ContractorId),

                CreatedOn = GetDateTime(entity, ApplicationFields.CreatedOn),
                ActivationDate = GetDateTime(entity, ApplicationFields.ActivationDate),

                ContractorPotentiallyEmerging = GetBool(entity, ApplicationFields.ContractorPotentiallyEmerging)
            };
        }

        public static Entity ToEntity(ApplicationModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entity = new Entity(CrmEntityNames.Application);

            // Only set Id for updates
            if (model.Id != Guid.Empty)
            {
                entity.Id = model.Id;
                entity[ApplicationFields.Id] = model.Id;
            }

            if (!string.IsNullOrWhiteSpace(model.ApplicationNumber))
                entity[ApplicationFields.ApplicationNumber] = model.ApplicationNumber;

            if (!string.IsNullOrWhiteSpace(model.Type))
                entity[ApplicationFields.Type] = model.Type;

            entity[ApplicationFields.Status] = model.StatusCode;
            entity[ApplicationFields.StateCode] = new OptionSetValue(model.StateCode);

            entity[ApplicationFields.ContractorId] =
                new EntityReference("account", model.ContractorId);

            entity[ApplicationFields.CreatedOn] = model.CreatedOn;

            entity[ApplicationFields.ActivationDate] = model.ActivationDate;

            entity[ApplicationFields.ContractorPotentiallyEmerging] = model.ContractorPotentiallyEmerging;

            return entity;
        }

        #region Helpers

        private static string? GetString(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<string>(key)
                : null;
        }

        private static Guid GetGuid(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<Guid>(key)
                : Guid.Empty;
        }

        private static DateTime GetDateTime(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<DateTime>(key)
                : default;
        }

        private static int GetOptionSet(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<OptionSetValue>(key)?.Value ?? 0
                : 0;
        }

        private static bool GetBool(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<bool>(key)
                : false;
        }

        #endregion
    }
}
