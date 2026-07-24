using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public class CredentialMapper
    {
        public static CredentialModel ToDomain(Entity e)
        {
            if (e == null) return null!;

            return new CredentialModel
            {
                Id = e.Id,
                Username = e.GetAttributeValue<string>(CredentialFields.Name),
                Password = e.GetAttributeValue<string>(CredentialFields.Password),
                LastLogin = e.GetAttributeValue<DateTime>(CredentialFields.LastLogin),
                IsLocked = e.GetAttributeValue<bool?>(CredentialFields.IsLocked) ?? false,
                MfaEnabled = e.GetAttributeValue<bool?>(CredentialFields.MFAEnabled) ?? false,

                //PreferredMfaMethod = e.GetAttributeValue<OptionSetValue>(CredentialFields.PreferredMfa)?.Value,
                IncorrectLoginCount = e.GetAttributeValue<int?>(CredentialFields.IncorrectLoginCount),

                ForcePasswordChange = e.GetAttributeValue<bool?>(CredentialFields.ForcePasswordChange) ?? false,

                //StatusCode = e.GetAttributeValue<OptionSetValue>(CredentialFields.StatusCode)?.Value
            };
        }

        public static Entity ToEntity(CredentialModel m)
        {
            var e = new Entity(CrmEntityNames.Credentials);

            if (m.Id != Guid.Empty)
                e.Id = m.Id;

            e[CredentialFields.Name] = m.Username;
            e[CredentialFields.Password] = m.Password;
            e[CredentialFields.IsLocked] = m.IsLocked;
            e[CredentialFields.LastLogin] = m.LastLogin;

            //if (m.PreferredMfaMethod.HasValue)
            //    e[CredentialFields.PreferredMfa] = new OptionSetValue(m.PreferredMfaMethod.Value);

            if (m.IncorrectLoginCount.HasValue)
                e[CredentialFields.IncorrectLoginCount] = m.IncorrectLoginCount.Value;

            e[CredentialFields.ForcePasswordChange] = m.ForcePasswordChange;

            //if (m.StatusCode.HasValue)
            //    e[CredentialFields.StatusCode] = new OptionSetValue(m.StatusCode.Value);

            return e;
        }
    }
}
