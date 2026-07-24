using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public static class ContactMapper
    {
        public static ContactModel ToDomain(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new ContactModel
            {
                Id = entity.Id,

                EmployerId = GetId(entity, ContactFields.Employer),
                Employer = entity.GetAttributeValue<EntityReference?>(ContactFields.Employer)?.Name,

                FirstName = GetString(entity, ContactFields.FirstName),
                LastName = GetString(entity, ContactFields.LastName),
                FullName = $"{GetString(entity, ContactFields.FirstName)} " + $"{GetString(entity, ContactFields.LastName)}".Trim(),

                Email = GetString(entity, ContactFields.Email),
                Telephone = GetString(entity, ContactFields.Telephone),
                FaxNumber = GetString(entity, ContactFields.FaxNumber),
                //TenderId = entity.GetAttributeValue<EntityReference?>(CrmEntityNames.Tender).Id,

                //IdNumber = GetString(entity, ContactFields.IdNumber),
                ContactType = GetOptionSetValue(entity, ContactFields.ContactType),

                //GenderCode = GetOptionSetLabel(entity, ContactFields.GenderCode),

                //RSACitizen = GetString(entity, ContactFields.RSACitizen),
                //IsBlack = GetBool(entity, ContactFields.IsBlack),

                CredentialsId = GetId(entity, ContactFields.CredentialsId),

                //CredentialsRequested = GetBool(entity, ContactFields.CredentialsRequested),

                Initials = GetString(entity, ContactFields.Initials),

                //Designation = GetString(entity, ContactFields.Designation),

                MobilePhone = GetString(entity, ContactFields.MobilePhone),
                AccountEnabled = GetBool(entity, ContactFields.AccountEnabled),
                Role = GetOptionSetLabel(entity, ContactFields.Role)
            }; 
        }

        public static Entity ToEntity(ContactModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entity = new Entity(CrmEntityNames.Contact);

            if (model.Id != Guid.Empty)
            {
                entity.Id = model.Id;
                entity[ContactFields.Id] = model.Id;
            }

            if (!string.IsNullOrWhiteSpace(model.FirstName))
                entity[ContactFields.FirstName] = model.FirstName;

            if (!string.IsNullOrWhiteSpace(model.LastName))
                entity[ContactFields.LastName] = model.LastName;

            if (!string.IsNullOrWhiteSpace(model.Email))
                entity[ContactFields.Email] = model.Email;

            if (!string.IsNullOrWhiteSpace(model.Telephone))
                entity[ContactFields.Telephone] = model.Telephone;

            if (!string.IsNullOrWhiteSpace(model.IdNumber))
                entity[ContactFields.IdNumber] = model.IdNumber;

            //if (model.ContactType.HasValue)
            //    entity[ContactFields.ContactType] = model.ContactType;

            if (!string.IsNullOrWhiteSpace(model.RSACitizen))
                entity[ContactFields.RSACitizen] = model.RSACitizen;

            if (model.IsBlack.HasValue)
                entity[ContactFields.IsBlack] = model.IsBlack.Value;

            if (model.CredentialsRequested.HasValue)
                entity[ContactFields.CredentialsRequested] = model.CredentialsRequested.Value;

            if (!string.IsNullOrWhiteSpace(model.Initials))
                entity[ContactFields.Initials] = model.Initials;

            if (!string.IsNullOrWhiteSpace(model.Designation))
                entity[ContactFields.Designation] = model.Designation;

            if (!string.IsNullOrWhiteSpace(model.MobilePhone))
                entity[ContactFields.MobilePhone] = model.MobilePhone;
            if(model.TenderId != Guid.Empty)
                entity["nv_tenderid"] = new EntityReference("nv_tender", model.TenderId);

            return entity;
        }
        #region Helpers

        private static string? GetString(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<string>(key)
                : null;
        }

        private static string? GetEntityReferenceId(Entity entity, string key)
        {
            if (!entity.Contains(key))
                return null;

            var reference = entity.GetAttributeValue<EntityReference>(key);

            return reference?.Id.ToString();
        }

        private static Guid? GetId(Entity entity, string key)
        {
            if (!entity.Contains(key))
                return null;

            var reference = entity.GetAttributeValue<EntityReference>(key);

            return reference?.Id;
        }

        private static string? GetOptionSetLabel(Entity entity, string key)
        {
            if (!entity.Contains(key))
                return null;

            var formattedValues = entity.FormattedValues;

            return formattedValues.Contains(key)
                ? formattedValues[key]
                : null;
        }

        private static int? GetOptionSetValue(Entity entity, string key)
        {
            return entity.Contains(key)
                ? entity.GetAttributeValue<OptionSetValue>(key)?.Value
                : null;
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
