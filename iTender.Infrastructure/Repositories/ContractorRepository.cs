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
    public class ContractorRepository : IContractorRepository
    {
        private readonly IOrganizationService _service;

        public ContractorRepository(ICrmServiceFactory crmFactory)
        {
            _service = crmFactory.Create();
        }

        public async Task<ContractorModel?> GetByCrsNumber(string crsNumber, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Account)
            {
                ColumnSet = new ColumnSet(true),
                Distinct = true,
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(CrmFieldNames.ContractorFields.CrsNumber, ConditionOperator.Equal, crsNumber),
                    }
                }
            };

            var entity = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities.FirstOrDefault(), ct);

            return entity == null
                ? null
                : ContractorMapper.ToDomain(entity);
        }

        public async Task<ContractorModel?> GetById(Guid id, CancellationToken ct = default)
        {
            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.Account,
                    id,
                    new ColumnSet(true)), ct);

            return entity == null
                ? null
                : ContractorMapper.ToDomain(entity);
        }

        public async Task<PagedResult<ContractorModel>> GetContractors(ContractorFilterModel filter, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Account)
            {
                ColumnSet = new ColumnSet(true),
                Distinct = true,
                PageInfo = new PagingInfo
                {
                    PageNumber = filter.PageNumber,
                    Count = filter.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var criteria = new FilterExpression(LogicalOperator.And);

            if (!string.IsNullOrWhiteSpace(filter.ProvinceId))
            {
                criteria.AddCondition(
                    ContractorFields.ProvinceId,
                    ConditionOperator.Equal,
                    filter.ProvinceId);
            }

            if (!string.IsNullOrWhiteSpace(filter.CRSNumber))
            {
                criteria.AddCondition(
                    ContractorFields.CrsNumber,
                    ConditionOperator.Equal,
                    filter.CRSNumber);
            }
            criteria.AddCondition(
                    ContractorFields.CrsNumber,
                    ConditionOperator.NotNull);

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                criteria.AddCondition(
                    ContractorFields.AddressCity,
                    ConditionOperator.Like,
                    $"%{filter.City}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.ContractorName))
            {
                criteria.AddCondition(
                    ContractorFields.Name,
                    ConditionOperator.Like,
                    $"%{filter.ContractorName}%");
            }

            var status = filter.Status ?? "Active";

            var statusCode = status.ToLower() switch
            {
                "active" => ContractorStatuses.Active,
                "suspended" => ContractorStatuses.Suspended,
                "deregistered" => ContractorStatuses.DeRegistered,
                "expired" => ContractorStatuses.Expired,
                "removed" => ContractorStatuses.Removed,
                _ => null
            };
            if (statusCode != null)
            {
                criteria.AddCondition(ContractorFields.StatusCode, ConditionOperator.Equal, int.Parse(statusCode));
            }

            query.Criteria = criteria;

            if (!string.IsNullOrWhiteSpace(filter.ClassOfConstructionWorksId) || filter.DesignationId != 0)
            {
                var link = new LinkEntity
                {
                    LinkFromEntityName = CrmEntityNames.Account,
                    LinkFromAttributeName = ContractorFields.Id,
                    LinkToEntityName = CrmEntityNames.ClassOfWork,
                    LinkToAttributeName = ConstructionContractContractorFields.ContractorId,
                    JoinOperator = JoinOperator.Inner,
                    Columns = new ColumnSet(true)
                };

                var linkCriteria = new FilterExpression(LogicalOperator.And);

                if (!string.IsNullOrWhiteSpace(filter.ClassOfConstructionWorksId))
                {
                    linkCriteria.AddCondition(
                        ClassOfConstructionWorkFields.Id,
                        ConditionOperator.Equal,
                        filter.ClassOfConstructionWorksId);
                }

                if (filter.ApprovedGrade.HasValue)
                {
                    linkCriteria.AddCondition(
                        ContractorGradeFields.ApprovedGrade,
                        ConditionOperator.Equal,
                        filter.ApprovedGrade.Value
                    );
                }

                link.LinkCriteria = linkCriteria;
                query.LinkEntities.Add(link);
            }

            var result = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var contractors = result.Entities
                .Select(ContractorMapper.ToDomain)
                .ToList();

            return new PagedResult<ContractorModel>
            {
                Items = contractors,
                TotalCount = result.TotalRecordCount
            };
        }
    }
}
