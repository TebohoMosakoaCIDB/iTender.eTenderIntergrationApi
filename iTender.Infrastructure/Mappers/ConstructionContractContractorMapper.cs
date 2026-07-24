using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;

namespace iTender.Infrastructure.Mappers
{
    public class ConstructionContractContractorMapper
    {
        public static Entity ToEntity(ConstructionContractContractorModel domain)
        {
            var entity = new Entity(CrmEntityNames.ConstrustionContractContractors);

            if (domain.ConstructionContractId.HasValue)
                entity[CrmFieldNames.ConstructionContractContractorFields.ConstructionContractId] = domain.ConstructionContractId.Value;

            if (domain.ContractorId.HasValue)
                entity[CrmFieldNames.ConstructionContractContractorFields.ContractorId] = domain.ContractorId.Value;

            if (domain.ValidCidbRegistration.HasValue)
                entity[CrmFieldNames.ConstructionContractContractorFields.ValidCidbRegistration] = domain.ValidCidbRegistration.Value;

            if (!string.IsNullOrEmpty(domain.Enterprisename))
                entity[CrmFieldNames.ConstructionContractContractorFields.EnterpriseName] = domain.Enterprisename;

            return entity;
        }

        public static ConstructionContractContractorModel? ToDomain(Entity entity)
        {
            if (entity == null) return null;

            return new ConstructionContractContractorModel
            {
                Id = entity.Id,

                ConstructionContractId =
                    entity.GetAttributeValue<EntityReference>(CrmFieldNames.ConstructionContractContractorFields.ConstructionContractId)?.Id,

                ContractorId =
                    entity.GetAttributeValue<EntityReference>(CrmFieldNames.ConstructionContractContractorFields.ContractorId)?.Id,

                ValidCidbRegistration =
                    entity.GetAttributeValue<OptionSetValue>(CrmFieldNames.ConstructionContractContractorFields.ValidCidbRegistration)?.Value == 100000000,

                Enterprisename =
                    entity.GetAttributeValue<string>(CrmFieldNames.ConstructionContractContractorFields.EnterpriseName)
            };
        }
    }
}
