using iTender.Application.DTOs;
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
    public class ContractorDevelopmentProgrammeRepository : IContractorDevelopmentProgrammeRepository
    {
        private readonly IOrganizationService _service;
        public ContractorDevelopmentProgrammeRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }
        public async Task<PagedResult<ContractorDevelopmentProgrammeModel>> GetAllCDPsForEmployer(CDPViewModel filter, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.ContractorDevelopmentProgramme)
            {
                ColumnSet = new ColumnSet(true),
                Distinct = true,
                Criteria = new FilterExpression(LogicalOperator.And),
                PageInfo = new PagingInfo
                {
                    PageNumber = filter.PageNumber,
                    Count = filter.PageSise,
                    ReturnTotalRecordCount = true
                }
            };

            query.Criteria.AddCondition(ContractorDevelopmentProgrammeFields.EmployerId, ConditionOperator.Equal, filter.EmployerId);

            var response = await Task.Run(
                () => _service.RetrieveMultiple(query),
                ct);

            return new PagedResult<ContractorDevelopmentProgrammeModel>
            {
                Items = response.Entities.Select(ContractorDevelopmentProgrammeMapper.ToDomain).ToList(),
                TotalCount = response.TotalRecordCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSise
            };
        }
    }
}
