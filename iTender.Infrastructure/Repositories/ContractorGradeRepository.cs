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
    public class ContractorGradeRepository : IContractorGradeRepository
    {
        private readonly IOrganizationService _service;
        public ContractorGradeRepository(ICrmServiceFactory crmFactory)
        {
            _service = crmFactory.Create();
        }
        public async Task<IEnumerable<ContractorGradeModel>> GetContractorGrades(Guid contractorId, Guid? classOfWorkTypeId = null, int? approvedGrade = null, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.ClassOfWork)
            {
                ColumnSet = new ColumnSet(
                    ContractorGradeFields.Id,
                    ContractorGradeFields.Name,
                    ContractorGradeFields.ContractorId,
                    ContractorGradeFields.ClassOfWorkTypeId,
                    ContractorGradeFields.ApprovedGrade,
                    ContractorGradeFields.ElectricalLicense,
                    ContractorGradeFields.CreatedOn,
                    ContractorGradeFields.ModifiedOn,
                    ContractorGradeFields.StateCode)
            };

            query.Criteria.AddCondition(
                ContractorGradeFields.ContractorId,
                ConditionOperator.Equal,
                contractorId);

            // Active records only
            query.Criteria.AddCondition(
                ContractorGradeFields.StateCode,
                ConditionOperator.Equal,
                0);

            if (classOfWorkTypeId.HasValue)
            {
                query.Criteria.AddCondition(
                    ContractorGradeFields.ClassOfWorkTypeId,
                    ConditionOperator.Equal,
                    classOfWorkTypeId.Value);
            }

            if (approvedGrade.HasValue)
            {
                query.Criteria.AddCondition(
                    ContractorGradeFields.ApprovedGrade,
                    ConditionOperator.Equal,
                    approvedGrade.Value);
            }

            var entities = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities, ct);

            return entities
                .Select(ContractorGradeMapper.ToDomain)
                .Where(x => x != null)!
                .Cast<ContractorGradeModel>();
        }
    }
}
