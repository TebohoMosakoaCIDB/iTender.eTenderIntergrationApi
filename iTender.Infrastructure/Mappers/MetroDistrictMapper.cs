using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public class MetroDistrictMapper
    {
        public static MetroDistrictModel ToDomain(Entity e)
        {
            if (e == null) return null!;

            return new MetroDistrictModel
            {
                Id = e.Id,
                ProvinceId = e.GetAttributeValue<EntityReference>(MetroDistrictFields.ProvinceId)?.Id,
                StateCode = e.GetAttributeValue<OptionSetValue>(MetroDistrictFields.StateCode)?.Value,
                Name = e.GetAttributeValue<string>(MetroDistrictFields.Name)
            };
        }

        public static Entity ToEntity(MetroDistrictModel m)
        {
            var e = new Entity(CrmEntityNames.MetroDistrict);

            if (m.Id != Guid.Empty)
                e.Id = m.Id;

            if (m.ProvinceId.HasValue)
                e[MetroDistrictFields.ProvinceId] = new EntityReference(MetroDistrictFields.ProvinceId, m.ProvinceId.Value);

            if (m.StateCode.HasValue)
                e[MetroDistrictFields.StateCode] = new OptionSetValue(m.StateCode.Value);

            e[MetroDistrictFields.Name] = m.Name;

            return e;
        }
    }
}
