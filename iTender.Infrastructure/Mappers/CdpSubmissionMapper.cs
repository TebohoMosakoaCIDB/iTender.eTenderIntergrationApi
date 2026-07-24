using iTender.Domain.Constants;
using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public static class CdpSubmissionMapper
    {
        public static CdpSubmissionModel? ToDomain(Entity entity)
        {
            if (entity == null) return null;

            return new CdpSubmissionModel
            {
                Id = entity.Id,

                CdpSubmissionId =
                    entity.GetAttributeValue<Guid>(CdpSubmissionFields.CdpSubmissionId),

                ContractorId =
                    entity.GetAttributeValue<EntityReference>(CdpSubmissionFields.ContractorCdpSubmissionId)?.Id,

                //CompetencyAssessmentPerformed =
                //    entity.GetAttributeValue<bool?>(CdpSubmissionFields.CompetencyAssessmentPerformed),

                //RemovedFromCdp =
                //    entity.GetAttributeValue<bool?>(CdpSubmissionFields.RemovedFromCdp),

                //TrainingRequirements =
                //    entity.GetAttributeValue<string>(CdpSubmissionFields.TrainingRequirements),

                //TargetClassOfWork =
                //    entity.GetAttributeValue<string>(CdpSubmissionFields.TargetClassOfWork),

                // These exist in your entity dump but NOT in your constants yet
                //ModifiedOn =
                //    entity.GetAttributeValue<DateTime>("modifiedon"),

                //CreatedOn =
                //    entity.GetAttributeValue<DateTime>("createdon"),

                StateCode =
                    entity.GetAttributeValue<OptionSetValue>("statecode")?.Value,

                StatusCode =
                    entity.GetAttributeValue<OptionSetValue>("statuscode")?.Value,

                //TimeZoneRuleVersionNumber =
                //    entity.GetAttributeValue<int?>("timezoneruleversionnumber"),

                //UtcConversionTimeZoneCode =
                //    entity.GetAttributeValue<int?>("utcconversiontimezonecode"),

                Name =
                    entity.GetAttributeValue<string>("nv_trainingrequirements") // adjust if you have a real name field
            };
        }
    }
}
