using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Collections;

namespace iTender.Infrastructure.Repositories
{
    public class FinancialStatementRepository : IFinancialStatementRepository
    {
        private readonly IOrganizationService _service;
        public FinancialStatementRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }
        public async Task<List<FinancialStatementModel>> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.FinancialStatement)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true
            };

            query.Criteria.AddCondition(
                CrmFieldNames.FinancialStatementFields.ApplicationId,
                ConditionOperator.Equal,
                applicationId);

            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

            query.AddOrder(
                CrmFieldNames.FinancialStatementFields.Year,
                OrderType.Descending);

            var results = _service.RetrieveMultiple(query);

            return results.Entities.Select(FinancialStatementMapper.ToDomain).ToList();
        }

        public async Task<List<FinancialStatementModel>> GetByContractorAsync(Guid contractorId, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.FinancialStatement)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true
            };

            query.Criteria.AddCondition(
                CrmFieldNames.FinancialStatementFields.ContractorId,
                ConditionOperator.Equal,
                contractorId);

            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

            query.AddOrder(
                CrmFieldNames.FinancialStatementFields.Year,
                OrderType.Descending);

            var results = _service.RetrieveMultiple(query);

            return results.Entities.Select(FinancialStatementMapper.ToDomain).ToList();
        }
    }
}
