using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;

namespace iTender.Infrastructure.Mappers
{
    public class PermissionMapper
    {
        public static Entity ToEntity(PermissionModel domain)
        {
            var entity = new Entity(CrmEntityNames.ContactPermissions);

            entity[CrmFieldNames.ContactPermissionFields.Id] = domain.PermissionId;

            if (!string.IsNullOrEmpty(domain.PermissionName))
                entity[CrmFieldNames.ContactPermissionFields.Name] = domain.PermissionName;

            entity[CrmFieldNames.ContactPermissionFields.ContactPermissionId] = domain.PermissionContactId;

            return entity;
        }

        public static PermissionModel? ToDomain(Entity entity)
        {
            if (entity == null) return null;

            return new PermissionModel
            {
                PermissionId = entity.Id,
                PermissionName = entity.GetAttributeValue<string>(CrmFieldNames.ContactPermissionFields.Name),
                PermissionContactId = entity.GetAttributeValue<EntityReference>(CrmFieldNames.ContactPermissionFields.ContactPermissionId)?.Id,
                IndividualId = entity.GetAttributeValue<EntityReference>(CrmFieldNames.ContactPermissionFields.IndividualId)?.Id ?? Guid.Empty
            };
        }
    }
}
