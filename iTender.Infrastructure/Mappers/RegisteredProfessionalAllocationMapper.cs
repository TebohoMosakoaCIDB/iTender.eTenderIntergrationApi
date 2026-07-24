using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;

namespace iTender.Infrastructure.Mappers
{
    public static class RegisteredProfessionalAllocationMapper
    {
        public static Entity ToEntity(RegisteredProfessionalAllocationModel domain)
        {
            var entity = new Entity("nv_registeredprofessionalallocation");

            if (domain.Id != Guid.Empty)
                entity.Id = domain.Id;

            if (domain.ApplicationId != Guid.Empty)
                entity["nv_applicationid"] = new EntityReference("nv_application", domain.ApplicationId);

            if (domain.PercentageWorkingTimeDevotedToEnterprise.HasValue)
                entity["nv_percentageworkingtimedevotedtoenterprise"] =
                    domain.PercentageWorkingTimeDevotedToEnterprise.Value;

            return entity;
        }

        public static RegisteredProfessionalAllocationModel? ToDomain(Entity entity)
        {
            if (entity == null) return null;

            return new RegisteredProfessionalAllocationModel
            {
                Id = entity.Id,

                ApplicationId =
                    entity.GetAttributeValue<EntityReference>("nv_applicationid")?.Id ?? Guid.Empty,

                ContractorId =
                    entity.GetAttributeValue<EntityReference>("nv_contractorid")?.Id ?? Guid.Empty,

                PercentageWorkingTimeDevotedToEnterprise =
                    entity.GetAttributeValue<int?>("nv_percentageworkingtimedevotedtoenterprise")
            };
        }
    }
}
