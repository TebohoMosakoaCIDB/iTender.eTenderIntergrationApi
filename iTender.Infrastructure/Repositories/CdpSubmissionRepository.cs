using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Repositories
{
    public class CdpSubmissionRepository : ICdpSubmissionRepository
    {
        private readonly IOrganizationService _service;
        public CdpSubmissionRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }

        public async Task<List<CdpSubmissionModel>> GetByCdpIdAsync(Guid cdpId, CancellationToken ct)
        {
            var query = new QueryExpression(CrmEntityNames.CdpSubmission)
            {
                ColumnSet = new ColumnSet(true),
            };

            var filter = new FilterExpression(LogicalOperator.And);

            filter.AddCondition(
                CdpSubmissionFields.CDPCdpSubmission,
                ConditionOperator.Equal,
                cdpId.ToString());

            filter.AddCondition(
                ContractorGradeFields.StateCode,
                ConditionOperator.NotEqual,
                StateCodes.StateCode_Inactive);

            query.Criteria = filter;

            var result = _service.RetrieveMultiple(query);

            return result.Entities
                .Select(CdpSubmissionMapper.ToDomain)
                .Where(x => x != null)
                .ToList()!;
        }
    }
}
