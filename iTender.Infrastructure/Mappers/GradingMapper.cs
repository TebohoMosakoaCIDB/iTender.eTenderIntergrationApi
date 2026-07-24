using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;

namespace iTender.Infrastructure.Mappers
{
    public static class GradingMapper
    {
        public static RecommendedGradeModel ToDomain(Entity entity)
        {
            return new RecommendedGradeModel
            {
                Grade = entity.GetAttributeValue<string>("nv_name"),

                TenderFromValue = entity.GetAttributeValue<Money>("nv_tenderfromvalue")?.Value,
                TenderToValue = entity.GetAttributeValue<Money>("nv_tendertovalue")?.Value,

                AdminFee = entity.GetAttributeValue<Money>("nv_adminfee")?.Value,
                AnnualFee = entity.GetAttributeValue<Money>("nv_annualfee")?.Value
            };
        }

        public static Entity ToEntity(RecommendedGradeModel domain)
        {
            var entity = new Entity("nv_name");

            entity["nv_grade"] = domain.Grade;

            if (domain.TenderFromValue.HasValue)
                entity["nv_tenderfromvalue"] = new Money(domain.TenderFromValue.Value);

            if (domain.TenderToValue.HasValue)
                entity["nv_tendertovalue"] = new Money(domain.TenderToValue.Value);

            if (domain.AdminFee.HasValue)
                entity["nv_adminfee"] = new Money(domain.AdminFee.Value);

            if (domain.AnnualFee.HasValue)
                entity["nv_annualfee"] = new Money(domain.AnnualFee.Value);

            return entity;
        }
    }
}
