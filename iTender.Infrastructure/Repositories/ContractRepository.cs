using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Enums;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly IOrganizationService _service;

        public ContractRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }
        public async Task<PagedResult<ContractModel>> AwardSearchAsync(AdvancedAwardSearchModel model, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (model.PageNo < 1) model.PageNo = 1;
            if (model.PageSize < 1) model.PageSize = 10;

            var query = new QueryExpression(CrmEntityNames.Contract)
            {
                ColumnSet = new ColumnSet(true),
                Distinct = true,
                PageInfo = new PagingInfo
                {
                    PageNumber = model.PageNo,
                    Count = model.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var andFilter = new FilterExpression(LogicalOperator.And);
            var orFilter = new FilterExpression(LogicalOperator.Or);

            andFilter.AddCondition(ContractorGradeFields.StateCode, ConditionOperator.NotEqual, 1);
            andFilter.AddCondition(ContractorFields.StatusCode, ConditionOperator.Equal, int.Parse(ContractStateCode.AWARDED_STATUS));
            andFilter.AddCondition(Contractfields.TenderId, ConditionOperator.NotNull);
            andFilter.AddCondition(Contractfields.DateOfPracticalCompletion, ConditionOperator.Null);
            andFilter.AddCondition(Contractfields.CIDBContractNumber, ConditionOperator.NotNull);
            andFilter.AddCondition(Contractfields.ContractNumber, ConditionOperator.NotNull);
            andFilter.AddCondition(Contractfields.EmployerContractID, ConditionOperator.NotNull);

            if (model.EmployerId.HasValue && model.EmployerId != Guid.Empty)
                andFilter.AddCondition(Contractfields.Employer_contract, ConditionOperator.Equal, model.EmployerId.Value);

            if (model.ProvinceID.HasValue)
                andFilter.AddCondition(Contractfields.ProvinceID, ConditionOperator.Equal, model.ProvinceID.Value);

            if (model.MetroDistMuniID.HasValue)
                andFilter.AddCondition(Contractfields.MetroDistrictMunicipalityID, ConditionOperator.Equal, model.MetroDistMuniID.Value);

            if (model.CoCW.HasValue)
                andFilter.AddCondition(Contractfields.ClassOfConstructionWorks, ConditionOperator.Equal, model.CoCW.Value);

            if (model.TenderValueRange.HasValue)
                andFilter.AddCondition(Contractfields.TenderValueRage, ConditionOperator.Equal, model.TenderValueRange.Value);

            if (!string.IsNullOrWhiteSpace(model.ContractTitle))
                andFilter.AddCondition(Contractfields.ContractTitle, ConditionOperator.Like, $"%{model.ContractTitle}%");

            if (!string.IsNullOrWhiteSpace(model.ContractDescr))
                andFilter.AddCondition(Contractfields.ContractDescription, ConditionOperator.Like, $"%{model.ContractDescr}%");

            if (!string.IsNullOrWhiteSpace(model.ContractNumber))
                andFilter.AddCondition(Contractfields.ContractNumber, ConditionOperator.Like, $"%{model.ContractNumber}%");

            if (!string.IsNullOrWhiteSpace(model.AwardFromDate))
                andFilter.AddCondition(Contractfields.ContractAwardDate, ConditionOperator.GreaterEqual, DateTime.Parse(model.AwardFromDate));

            if (!string.IsNullOrWhiteSpace(model.AwardBeforeDate))
                andFilter.AddCondition(Contractfields.ContractAwardDate, ConditionOperator.LessEqual, DateTime.Parse(model.AwardBeforeDate));

            if (!string.IsNullOrWhiteSpace(model.TenderNumber))
            {
                var link = new LinkEntity
                {
                    LinkFromEntityName = CrmEntityNames.Contract,
                    LinkFromAttributeName = Contractfields.TenderId,
                    LinkToEntityName = CrmEntityNames.Tender,
                    LinkToAttributeName = TenderFields.Id,
                    JoinOperator = JoinOperator.Inner,
                    LinkCriteria = new FilterExpression
                    {
                        Conditions =
                {
                    new ConditionExpression(
                        TenderFields.EmployerTenderNumber,
                        ConditionOperator.Like,
                        $"%{model.TenderNumber}%")
                }
                    }
                };

                query.LinkEntities.Add(link);
            }

            query.Criteria.AddFilter(andFilter);
            query.Criteria.AddFilter(orFilter);

            if (model.SortBycolumn != null && model.SortBycolumn.Length > 0)
            {
                foreach (var col in model.SortBycolumn)
                {
                    query.Orders.Add(new OrderExpression(col, OrderType.Ascending));
                }
            }
            else
            {
                query.Orders.Add(new OrderExpression(ApplicationFields.CreatedOn, OrderType.Descending));
            }

            ct.ThrowIfCancellationRequested();

            // Async call (Dataverse)
            var result =  _service.RetrieveMultiple(query);

            ct.ThrowIfCancellationRequested();

            return new PagedResult<ContractModel>
            {
                Items = result.Entities.Select(ContractMapper.ToDomain).ToList(),
                TotalCount = result.TotalRecordCount,
                PageNumber = 1
            };
        }

        public bool CheckForDuplicateContractNumbers(Guid employerId, string contractNumber, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var query = new QueryExpression(CrmEntityNames.Contract)
            {
                ColumnSet = new ColumnSet(false),
                TopCount = 1
            };

            query.Criteria.AddCondition(
                Contractfields.Employer_contract,
                ConditionOperator.Equal,
                employerId
            );

            query.Criteria.AddCondition(
                Contractfields.ContractNumber,
                ConditionOperator.Equal,
                contractNumber
            );

            var result = _service.RetrieveMultiple(query);
            ct.ThrowIfCancellationRequested();

            return result.Entities.Any();
        }

        public ContractModel GetContractById(Guid contractId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (contractId == Guid.Empty)
                return null;

            var entity = _service.Retrieve(
                CrmEntityNames.Contract,
                contractId,
                new ColumnSet(true)
            );
            ct.ThrowIfCancellationRequested();

            return entity != null
                ? ContractMapper.ToDomain(entity)
                : null;
        }

        public List<ContractModel> GetContractsAwarded(Guid employerId,bool isPracticallyComplete, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var query = new QueryExpression(CrmEntityNames.Contract)
            {
                ColumnSet = new ColumnSet(true),
                Distinct = true
            };

            var filter = new FilterExpression(LogicalOperator.And);

            // 🔹 Employer (GUID-safe)
            filter.AddCondition(
                Contractfields.Employer_contract,
                ConditionOperator.Equal,
                employerId
            );

            // 🔹 Status = Awarded
            filter.AddCondition(
                Contractfields.StatusCode,
                ConditionOperator.Equal,
                ContractStateCode.AWARDED_STATUS
            );

            // 🔹 Active only (not inactive)
            filter.AddCondition(
                Contractfields.StateCode,
                ConditionOperator.NotEqual,
                StateCodes.StateCode_Inactive
            );

            // 🔹 Practical completion logic (matches OLD implementation exactly)
            filter.AddCondition(
                Contractfields.DateOfPracticalCompletion,
                isPracticallyComplete
                    ? ConditionOperator.Null        // IMPORTANT: preserved from old logic
                    : ConditionOperator.NotNull
            );

            query.Criteria = filter;

            // 🔹 Reintroduce missing relationship join (critical in old version)
            var contractorLink = new LinkEntity
            {
                LinkFromEntityName = CrmEntityNames.Contract,
                LinkFromAttributeName = Contractfields.ContractId,
                LinkToEntityName = "nv_constructioncontractcontractors",
                LinkToAttributeName = "nv_constructioncontractid",
                JoinOperator = JoinOperator.Inner
            };

            query.LinkEntities.Add(contractorLink);

            // 🔹 Execute safely
            var result = _service.RetrieveMultiple(query);

            ct.ThrowIfCancellationRequested();

            // 🔹 Defensive mapping (avoid null entity issues)
            return result.Entities?
                .Where(e => e != null)
                .Select(ContractMapper.ToDomain)
                .ToList()
                ?? new List<ContractModel>();
        }

        public List<ContractModel> GetContractsByContractNumber(Guid employerId, string contractNumber, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var query = new QueryExpression(CrmEntityNames.Contract)
            {
                ColumnSet = new ColumnSet(true)
            };

            query.Criteria.AddCondition(
                Contractfields.Employer_contract,
                ConditionOperator.Equal,
                employerId
            );

            query.Criteria.AddCondition(
                Contractfields.ContractNumber,
                ConditionOperator.Like,
                $"%{contractNumber}%"
            );

            var result = _service.RetrieveMultiple(query);
            ct.ThrowIfCancellationRequested();
            return result.Entities
                .Select(ContractMapper.ToDomain)
                .ToList();
        }

        public async Task<PagedResult<ContractModel>> GetContracts(ContractFilterViewModel filter, Guid employerId, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Contract)
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

            // Common filters
            query.Criteria.AddCondition(Contractfields.EmployerContractID, ConditionOperator.Equal, employerId);

            query.Criteria.AddCondition(Contractfields.StateCode, ConditionOperator.NotEqual, StateCodes.StateCode_Inactive);

            // Link Entity (used by all existing methods)
            query.LinkEntities.Add(new LinkEntity
            {
                JoinOperator = JoinOperator.Natural,
                LinkFromAttributeName = Contractfields.ContractId,
                LinkFromEntityName = CrmEntityNames.Contract,
                LinkToAttributeName = "nv_constructioncontractid",
                LinkToEntityName = "nv_constructioncontractcontractors"
            });

            #region Search Filters

            if (!string.IsNullOrWhiteSpace(filter.ContractNumber))
            {
                query.Criteria.AddCondition(Contractfields.ContractNumber, ConditionOperator.Like, $"%{filter.ContractNumber}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.CIDBRefenceNumber))
            {
                query.Criteria.AddCondition(Contractfields.CIDBContractNumber, ConditionOperator.Like, $"%{filter.CIDBRefenceNumber}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.ContractTitle))
            {
                query.Criteria.AddCondition(Contractfields.ContractTitle, ConditionOperator.Like, $"%{filter.ContractTitle}%");
            }
            if (!string.IsNullOrWhiteSpace(filter.ContractDescription))
            {
                query.Criteria.AddCondition(Contractfields.ContractDescription, ConditionOperator.Like, $"%{filter.ContractDescription}%");
            }

            #endregion

            #region User Filter

            if (filter.User.HasValue && filter.UserId.HasValue)
            {
                switch (filter.User.Value)
                {
                    case UserFilter.CreatedBy:
                        query.Criteria.AddCondition(
                            "createdby",
                            ConditionOperator.Equal,
                            filter.UserId.Value);
                        break;

                    case UserFilter.LastNodifiedBy:
                        query.Criteria.AddCondition(
                            "modifiedby",
                            ConditionOperator.Equal,
                            filter.UserId.Value);
                        break;
                }
            }

            #endregion

            #region Status Filter

            if (filter.Status.HasValue)
            {
                switch (filter.Status.Value)
                {
                    case ContractStatusFilter.ActiveContracts:
                        query.Criteria.AddCondition(Contractfields.DateOfPracticalCompletion, ConditionOperator.Null);
                        query.Criteria.AddCondition(Contractfields.StatusCode,ConditionOperator.Equal, ContractStateCode.AWARDED_STATUS);
                        query.Criteria.AddCondition(
                            Contractfields.ChangeRequestStatus,
                            ConditionOperator.NotIn,
                            ContractStateCode.PENDING,
                            ContractStateCode.APPROVED,
                            ContractStateCode.DECLINED,
                            ContractStateCode.DRAFT);

                        break;

                    case ContractStatusFilter.PendingContractChangeRequests:
                        query.Criteria.AddCondition(
                            Contractfields.ChangeRequestStatus,
                            ConditionOperator.In,
                            ContractStateCode.PENDING,
                            ContractStateCode.APPROVED,
                            ContractStateCode.DECLINED,
                            ContractStateCode.DRAFT);
                        break;

                    case ContractStatusFilter.ContractsWherePraticalCompletionHasBeenRegistered:
                        query.Criteria.AddCondition(Contractfields.DateOfPracticalCompletion, ConditionOperator.NotNull);
                        query.Criteria.AddCondition(Contractfields.StatusCode, ConditionOperator.Equal, ContractStateCode.AWARDED_STATUS);
                        break;

                    case ContractStatusFilter.ContractsTerminated:
                        query.Criteria.AddCondition(Contractfields.TerminationDate, ConditionOperator.NotNull);
                        break;
                }
            }

            #endregion

            var response = await Task.Run(
                () => _service.RetrieveMultiple(query),
                ct);

            return new PagedResult<ContractModel>
            {
                Items = response.Entities
                    .Select(ContractMapper.ToDomain)
                    .ToList(),
                TotalCount = response.TotalRecordCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSise
            };
        }
    }
}
