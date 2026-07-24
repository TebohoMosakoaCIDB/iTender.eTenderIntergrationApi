using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public static class ClassOfConstructionWorkMapper
    {
        public static Entity ToEntity(ClassOfConstructionWorkModel domain)
        {
            var entity = new Entity("nv_classofworktype");

            if (!string.IsNullOrEmpty(domain.Name))
                entity[ClassOfConstructionWorkFields.Name] = domain.Name;

            if (!string.IsNullOrEmpty(domain.Description))
                entity[ClassOfConstructionWorkFields.Description] = domain.Description;

            if (domain.SourceId.HasValue)
                entity[ClassOfConstructionWorkFields.SourceId] = domain.SourceId.Value;

            return entity;
        }

        public static ClassOfConstructionWorkModel? ToDomain(Entity entity)
        {
            if (entity == null) return null;

            return new ClassOfConstructionWorkModel
            {
                Id = entity.Id,

                Name = entity.GetAttributeValue<string>("nv_name"),
                Description = entity.GetAttributeValue<string>("nv_description"),
                SourceId = entity.GetAttributeValue<int?>("nv_class_of_worktype_sourceid")
            };
        }
    }
}
