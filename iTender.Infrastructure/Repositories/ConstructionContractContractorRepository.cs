using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace iTender.Infrastructure.Repositories
{
    public class ConstructionContractContractorRepository : IConstructionContractContractorRepository
    {
        private readonly IOrganizationService _service;

        public ConstructionContractContractorRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }

        public async Task<IEnumerable<ConstructionContractContractorModel>> GetByContractId(Guid contractId)
        {
            var query = new QueryExpression(CrmEntityNames.ConstrustionContractContractors)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And),
                NoLock = true
            };

            query.Criteria.AddCondition(
                CrmFieldNames.ConstructionContractContractorFields.ConstructionContractId, 
                ConditionOperator.Equal,
                contractId);

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query));

            return response.Entities
                .Select(ConstructionContractContractorMapper.ToDomain);
        }

        public async Task<IEnumerable<ConstructionContractContractorModel>> GetByContractorId(Guid contractorId)
        {
            var query = new QueryExpression(CrmEntityNames.ConstrustionContractContractors)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And),
                NoLock = true
            };

            query.Criteria.AddCondition(
                CrmFieldNames.ConstructionContractContractorFields.ContractorId,
                ConditionOperator.Equal,
                contractorId);

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query));

            return response.Entities
                .Select(ConstructionContractContractorMapper.ToDomain);
        }
    }
}