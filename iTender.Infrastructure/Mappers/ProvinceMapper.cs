using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;

namespace iTender.Infrastructure.Mappers
{
    public class ProvinceMapper
    {
        public static ProvinceModel ToDomain(Entity e)
        {
            if (e == null) return null!;

            return new ProvinceModel
            {
                Id = e.Id,
                Name = e.GetAttributeValue<string>(CrmFieldNames.ProvinceFields.Name),
            };
        }

        public static Entity ToEntity(ProvinceModel m)
        {
            var e = new Entity(CrmEntityNames.Province);

            if (m.Id != Guid.Empty)
                e.Id = m.Id;
            e[CrmFieldNames.ProvinceFields.Name] = m.Name;

            return e;
        }
    }
}
