using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace iTender.Infrastructure.Repositories
{
    public class GradingRepository : IGradingRepository
    {
        private readonly IOrganizationService _service;

        public GradingRepository(ICrmServiceFactory crmFactory)
        {
            _service = crmFactory.Create();
        }

        public async Task<RecommendedGradeModel> GetByGradeAsync(string grade)
        {
            var query = new QueryExpression(CrmEntityNames.Grade)
            {
                ColumnSet = new ColumnSet(true)
            };

            query.Criteria = new FilterExpression(LogicalOperator.And);
            query.Criteria.AddCondition("nv_name", ConditionOperator.Equal, grade);
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0); // ACTIVE only (important!)

            var result = _service.RetrieveMultiple(query);

            var entity = result.Entities.FirstOrDefault();

            return entity == null ? null : GradingMapper.ToDomain(entity);
        }
    }
}
