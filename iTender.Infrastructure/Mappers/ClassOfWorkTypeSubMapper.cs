using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace iTender.Infrastructure.Mappers
{
    public static class ClassOfWorkTypeSubMapper
    {
        public static ClassOfWorkTypeSubModel ToDomain(Entity e)
        {
            if (e == null) return null;

            return new ClassOfWorkTypeSubModel
            {
                Id = e.Id,
                ClassOfWorkTypeId = e.GetAttributeValue<EntityReference>(CrmFieldNames.ClassOfConstructionWorkSubCategoryFields.ClassOfWorkTypeId)?.Id,
                Name = e.GetAttributeValue<string>(CrmFieldNames.ClassOfConstructionWorkSubCategoryFields.Name)
            };
        }

        public static Entity ToEntity(ClassOfWorkTypeSubModel m)
        {
            var e = new Entity(CrmEntityNames.ClassOfWorkTypeSub);

            if (m.Id != Guid.Empty)
                e.Id = m.Id;
            if (m.ClassOfWorkTypeId.HasValue)
                e[CrmFieldNames.ClassOfConstructionWorkSubCategoryFields.ClassOfWorkTypeId] = new EntityReference(CrmFieldNames.ClassOfConstructionWorkSubCategoryFields.ClassOfWorkTypeId, m.ClassOfWorkTypeId.Value);
            e[CrmFieldNames.ClassOfConstructionWorkSubCategoryFields.Name] = m.Name;

            return e;
        }
    }
}
