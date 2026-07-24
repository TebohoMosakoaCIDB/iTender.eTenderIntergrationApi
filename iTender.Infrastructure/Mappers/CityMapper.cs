using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;

namespace iTender.Infrastructure.Mappers
{
    public static class CityMapper
    {
        public static Entity ToEntity(CityModel domain)
        {
            var entity = new Entity(CrmEntityNames.City);

            entity[CrmFieldNames.CityFields.Id] = domain.Id;    

            if (!string.IsNullOrEmpty(domain.Name))
                entity[CrmFieldNames.CityFields.Name] = domain.Name;

            if (domain.ProvinceId.HasValue)
                entity[CrmFieldNames.CityFields.ProvinceId] = domain.ProvinceId.Value;

            return entity;
        }

        public static CityModel? ToDomain(Entity entity)
        {
            if (entity == null) return null;

            return new CityModel
            {
                Id = entity.Id,
                Name = entity.GetAttributeValue<string>(CrmFieldNames.CityFields.Name),
                ProvinceId = entity.GetAttributeValue<EntityReference>(CrmFieldNames.CityFields.ProvinceId)?.Id
            };
        }
    }
}
