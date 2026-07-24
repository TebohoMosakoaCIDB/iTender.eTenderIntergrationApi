using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Enums;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.ServiceModel;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Repositories
{
    public class TenderRepository : ITenderRepository
    {
        private readonly IOrganizationService _service;
        public TenderRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }

        public PagedResult<TenderModel> GetAdvancedFilteredTenders(AdvancedTenderSearchViewModel filter, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            filter ??= new AdvancedTenderSearchViewModel();

            var query = new QueryExpression(CrmEntityNames.Tender)
            {
                ColumnSet = new ColumnSet(true),
                Distinct = true,
                Criteria = new FilterExpression(LogicalOperator.And),
                PageInfo = new PagingInfo
                {
                    PageNumber = filter.PageNumber,
                    Count = filter.numberOfResultsPerPage >= 0 ? 10 : filter.numberOfResultsPerPage,
                    ReturnTotalRecordCount = true
                }
            };

            var conditions = new List<ConditionExpression>();

            conditions.Add(new ConditionExpression(TenderFields.StateCode, ConditionOperator.NotEqual, StateCodes.StateCode_Inactive));
            conditions.Add(new ConditionExpression(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.ADVERTISED_STATUS));
            conditions.Add(new ConditionExpression(TenderFields.DateAdvertised, ConditionOperator.NotNull));
            conditions.Add(new ConditionExpression(TenderFields.EmployerId, ConditionOperator.NotNull));
            conditions.Add(new ConditionExpression(TenderFields.IsClosed, ConditionOperator.Equal, "0"));

            if (!string.IsNullOrEmpty(filter.NoticeType))
            {
                if (filter.NoticeType.ToLower() == "eoi")
                {
                    conditions.Add(new ConditionExpression(
                        TenderFields.IsEOI,
                        ConditionOperator.Equal,
                        TenderState.EOI_CRM_VALUE
                    ));
                }
                if (filter.NoticeType.ToLower() == "tender")
                {
                    conditions.Add(new ConditionExpression(
                        TenderFields.IsEOI,
                        ConditionOperator.Equal,
                        TenderState.TENDER_CRM_VALUE
                    ));
                }
            }

            if (!string.IsNullOrWhiteSpace(filter.TenderNumber))
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.EmployerTenderNumber,
                    ConditionOperator.Like,
                    filter.TenderNumber
                ));
            }

            if (!string.IsNullOrWhiteSpace(filter.BatchReferenceNumber))
            {
                conditions.Add(new ConditionExpression(
                   TenderFields.BatchReferenceNumber,
                   ConditionOperator.Like,
                   filter.BatchReferenceNumber
                ));
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                conditions.Add(new ConditionExpression(
                   TenderFields.Title,
                   ConditionOperator.Like,
                   filter.Description
                ));
            }

            if (filter.EmployerId.HasValue)
            {
                conditions.Add(new ConditionExpression(
                   TenderFields.EmployerId,
                   ConditionOperator.Equal,
                   filter.EmployerId
                ));
            }

            if (filter.MetroDistrictId.HasValue)
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.MetroDistrictId,
                    ConditionOperator.Equal,
                    filter.MetroDistrictId.Value
                ));
            }

            if (filter.ProvinceId.HasValue &&
                filter.ProvinceId != Guid.Empty)
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.ProvinceId,
                    ConditionOperator.Equal,
                    filter.ProvinceId.Value
                ));
            }

            if (filter.ClassOfConstructionWorksId.HasValue || filter.AlternateClassOfConstructionWorksId.HasValue)
            {
                var classFilter = new FilterExpression(LogicalOperator.Or);

                if (filter.ClassOfConstructionWorksId.HasValue)
                {
                    classFilter.AddCondition(TenderFields.ClassOfWork,
                        ConditionOperator.Equal,
                        filter.ClassOfConstructionWorksId.Value);
                }

                if (filter.AlternateClassOfConstructionWorksId.HasValue)
                {
                    classFilter.AddCondition(TenderFields.AltClassOfWork,
                        ConditionOperator.Equal,
                        filter.AlternateClassOfConstructionWorksId.Value);
                }

                query.Criteria.AddFilter(classFilter);
            }

            if (filter.ClassOfConstructionWorksSubCategoryId.HasValue || filter.AlternateClassOfConstructionWorksSubCategoryId.HasValue)
            {
                var subFilter = new FilterExpression(LogicalOperator.Or);

                if (filter.ClassOfConstructionWorksSubCategoryId.HasValue)
                {
                    subFilter.AddCondition(TenderFields.SubCategory,
                        ConditionOperator.Equal,
                        filter.ClassOfConstructionWorksSubCategoryId.Value);
                }

                if (filter.AlternateClassOfConstructionWorksSubCategoryId.HasValue)
                {
                    subFilter.AddCondition(TenderFields.AltClassOfConstructionWorksSubId,
                        ConditionOperator.Equal,
                        filter.AlternateClassOfConstructionWorksSubCategoryId.Value);
                }

                query.Criteria.AddFilter(subFilter);
            }

            if (filter.TendersClosingBefore.HasValue)
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.ClosingDate,
                    ConditionOperator.LessEqual,
                    filter.TendersClosingBefore.Value
                ));
            }
            else
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.ClosingDate,
                    ConditionOperator.GreaterThan,
                    DateTime.Now
                ));
            }

            query.Criteria.Conditions.AddRange(conditions);

            ct.ThrowIfCancellationRequested();

            var result = _service.RetrieveMultiple(query);

            ct.ThrowIfCancellationRequested();

            return new PagedResult<TenderModel>
            {
                Items = result.Entities.Select(TenderMapper.ToDomain).ToList(),
                TotalCount = result.TotalRecordCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.numberOfResultsPerPage
            };
        }
        
        public async Task<PagedResult<TenderModel>> GetTenders(TenderFilterViewModel filter, Guid employerId, TenderType tenderType, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Tender)
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
            query.Criteria.AddCondition(TenderFields.EmployerId, ConditionOperator.Equal, employerId);

            if (filter.Status != TenderStatusFilter.TenderCancelled)
            {
                query.Criteria.AddCondition(
                    TenderFields.StateCode,
                    ConditionOperator.NotEqual,
                    StateCodes.StateCode_Inactive);
            }

            query.Criteria.AddCondition(TenderFields.IsEOI, ConditionOperator.Equal, tenderType == TenderType.EOI ? TenderState.EOI_CRM_VALUE : TenderState.TENDER_CRM_VALUE);

            #region Search Filters

            if (!string.IsNullOrWhiteSpace(filter.CIDBRefenceNumber))
            {
                query.Criteria.AddCondition(TenderFields.CidbReferencenNumber, ConditionOperator.Like, $"%{filter.CIDBRefenceNumber}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.TenderNumber))
            {
                query.Criteria.AddCondition(TenderFields.EmployerTenderNumber, ConditionOperator.Like, $"%{filter.TenderNumber}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                query.Criteria.AddCondition(TenderFields.Title, ConditionOperator.Like, $"%{filter.Description}%");
            }

            #endregion

            #region User Filter

            if (filter.User.HasValue && filter.UserId.HasValue)
            {
                switch (filter.User.Value)
                {
                    case UserFilter.CreatedBy:
                        query.Criteria.AddCondition("createdby", ConditionOperator.Equal, filter.UserId.Value);
                        break;

                    case UserFilter.LastNodifiedBy:
                        query.Criteria.AddCondition("modifiedby", ConditionOperator.Equal, filter.UserId.Value);
                        break;
                }
            }

            #endregion

            #region Status Filter

            if (filter.Status.HasValue)
            {
                switch (filter.Status.Value)
                {
                    case TenderStatusFilter.SavedNotAdvertised:
                        query.Criteria.AddCondition(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.DRAFT_STATUS);
                        query.Criteria.AddCondition(TenderFields.ChangeRequestStatus, ConditionOperator.NotIn, TenderState.APPROVED_CHANGEREQUEST_STATUS, TenderState.DECLINED_CHANGEREQUEST_STATUS, TenderState.PENDING_CHANGEREQUEST_STATUS);
                        break;

                    case TenderStatusFilter.AdvertisedNotClosed:
                        query.Criteria.AddCondition(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.ADVERTISED_STATUS);
                        query.Criteria.AddCondition(TenderFields.IsClosed, ConditionOperator.Equal, "0");
                        query.Criteria.AddCondition(TenderFields.DateAdvertised, ConditionOperator.NotNull);
                        query.Criteria.AddCondition(TenderFields.ClosingDate, ConditionOperator.GreaterThan, DateTime.Now);
                        break;

                    case TenderStatusFilter.ClosedNotAwarded:
                        query.Criteria.AddCondition(TenderFields.Awarded, ConditionOperator.Equal, "0");
                        query.Criteria.AddCondition(TenderFields.ClosingDate, ConditionOperator.LessEqual, DateTime.Now);
                        query.Criteria.AddCondition(TenderFields.IsClosed, ConditionOperator.Equal, "1");
                        query.Criteria.AddCondition(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.ADVERTISED_STATUS);
                        break;

                    case TenderStatusFilter.TenderAwarded:
                        query.Criteria.AddCondition(TenderFields.Awarded, ConditionOperator.Equal, "1");
                        query.Criteria.AddCondition(TenderFields.IsClosed, ConditionOperator.Equal, "1");
                        query.Criteria.AddCondition(TenderFields.ClosingDate, ConditionOperator.LessEqual, DateTime.Now);
                        query.Criteria.AddCondition(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.ADVERTISED_STATUS);
                        break;

                    case TenderStatusFilter.PendingChangeRequestNotAdvertised:
                        query.Criteria.AddCondition(TenderFields.ChangeRequestStatus, ConditionOperator.In, TenderState.APPROVED_CHANGEREQUEST_STATUS, TenderState.DECLINED_CHANGEREQUEST_STATUS, TenderState.PENDING_CHANGEREQUEST_STATUS);
                        query.Criteria.AddCondition(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.DRAFT_STATUS);
                        break;

                    case TenderStatusFilter.TenderCancelled:
                        query.Criteria.AddCondition(TenderFields.StateCode, ConditionOperator.Equal, StateCodes.StateCode_Inactive);
                        query.Criteria.AddCondition(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.CANCELLED_STATUS);
                        break;
                }
            }

            #endregion

            var response = await Task.Run(
                () => _service.RetrieveMultiple(query),
                ct);

            return new PagedResult<TenderModel>
            {
                Items = response.Entities.Select(TenderMapper.ToDomain).ToList(),
                TotalCount = response.TotalRecordCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSise
            };
        }

        public PagedResult<TenderModel> GetByFilter(TenderFilterModel? filter, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            filter ??= new TenderFilterModel();

            var query = new QueryExpression(CrmEntityNames.Tender)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And),
                PageInfo = new PagingInfo
                {
                    PageNumber = filter.PageNumber,
                    Count = filter.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var conditions = new List<ConditionExpression>();

            conditions.Add(new ConditionExpression(TenderFields.StateCode, ConditionOperator.NotEqual, StateCodes.StateCode_Inactive));
            conditions.Add(new ConditionExpression(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.ADVERTISED_STATUS));
            conditions.Add(new ConditionExpression(TenderFields.DateAdvertised, ConditionOperator.NotNull));
            conditions.Add(new ConditionExpression(TenderFields.EmployerId, ConditionOperator.NotNull));
            conditions.Add(new ConditionExpression(TenderFields.IsEOI, ConditionOperator.Equal, TenderState.TENDER_CRM_VALUE));
            conditions.Add(new ConditionExpression(TenderFields.IsClosed, ConditionOperator.Equal, "0"));
            conditions.Add(new ConditionExpression(TenderFields.ClosingDate, ConditionOperator.GreaterThan, DateTime.Now));

            if (filter.ProvinceId.HasValue && filter.ProvinceId != Guid.Empty)
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.ProvinceId,
                    ConditionOperator.Equal,
                    filter.ProvinceId.Value
                ));
            }

            if (filter.ClassOfConstructionWorksId.HasValue && filter.ClassOfConstructionWorksId != Guid.Empty)
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.ClassOfWork,
                    ConditionOperator.Equal,
                    filter.ClassOfConstructionWorksId.Value
                ));
            }

            if (filter.AlternateClassOfConstructionWorksId.HasValue &&
                filter.AlternateClassOfConstructionWorksId != Guid.Empty)
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.AltClassOfWork,
                    ConditionOperator.Equal,
                    filter.AlternateClassOfConstructionWorksId.Value
                ));
            }

            if (filter.DesignationId.HasValue)
            {
                conditions.Add(new ConditionExpression(
                    TenderFields.TenderValueRange,
                    ConditionOperator.Equal,
                    filter.DesignationId
                ));
            }

            query.Criteria.Conditions.AddRange(conditions);

            ct.ThrowIfCancellationRequested();

            var result = _service.RetrieveMultiple(query);

            ct.ThrowIfCancellationRequested();

            return new PagedResult<TenderModel>
            {
                Items = result.Entities.Select(TenderMapper.ToDomain).ToList(),
                TotalCount = result.TotalRecordCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public List<ProvinceStatViewModel> GetTenderCountByProvince(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var results = new List<Entity>();
            string? pagingCookie = null;
            int pageNumber = 1;

            do
            {
                ct.ThrowIfCancellationRequested();

                var query = new QueryExpression(CrmEntityNames.Tender)
                {
                    ColumnSet = new ColumnSet(TenderFields.ProvinceId),
                    PageInfo = new PagingInfo
                    {
                        PageNumber = pageNumber,
                        Count = 5000,
                        PagingCookie = pagingCookie
                    }
                };

                var criteria = new FilterExpression(LogicalOperator.And);

                criteria.AddCondition(
                    TenderFields.StatusCode,
                    ConditionOperator.Equal,
                    TenderState.ADVERTISED_STATUS);

                criteria.AddCondition(
                    TenderFields.StateCode,
                    ConditionOperator.NotEqual,
                    StateCodes.StateCode_Inactive);

                criteria.AddCondition(
                    TenderFields.DateAdvertised,
                    ConditionOperator.NotNull);

                criteria.AddCondition(
                    TenderFields.EmployerId,
                    ConditionOperator.NotNull);

                criteria.AddCondition(
                    TenderFields.IsEOI,
                    ConditionOperator.Equal,
                    TenderState.TENDER_CRM_VALUE);

                criteria.AddCondition(
                    TenderFields.IsClosed,
                    ConditionOperator.Equal,
                    "0");

                criteria.AddCondition(
                    TenderFields.ClosingDate,
                    ConditionOperator.GreaterThan,
                    DateTime.Now);

                query.Criteria = criteria;

                var response = _service.RetrieveMultiple(query);

                results.AddRange(response.Entities);

                pagingCookie = response.PagingCookie;
                pageNumber++;

                if (!response.MoreRecords)
                    break;

            } while (true);

            return results
                .Where(e => e.Contains(TenderFields.ProvinceId))
                .GroupBy(e =>
                {
                    var province = e.GetAttributeValue<EntityReference>(TenderFields.ProvinceId);

                    return new
                    {
                        province.Id,
                        province.Name
                    };
                })
                .Select(g => new ProvinceStatViewModel
                {
                    ProvinceId = g.Key.Id,
                    ProvinceName = g.Key.Name,
                    Count = g.Count()
                })
                .ToList();
        }

        public TenderSummaryViewModel GetTenderSummary(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = new TenderSummaryViewModel
            {
                Designations = Enumerable.Range(1, 9)
                    .Select(x => x.ToString())
                    .ToList()
            };

            var query = new QueryExpression(CrmEntityNames.Tender)
            {
                ColumnSet = new ColumnSet(
                    TenderFields.ClassOfWork,
                    TenderFields.TenderValueRange
                ),
                Criteria = new FilterExpression(LogicalOperator.And)
            };

            query.Criteria.Conditions.AddRange(new List<ConditionExpression>
            {
                new ConditionExpression(TenderFields.StateCode, ConditionOperator.NotEqual, StateCodes.StateCode_Inactive),
                new ConditionExpression(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.ADVERTISED_STATUS),
                new ConditionExpression(TenderFields.DateAdvertised, ConditionOperator.NotNull),
                new ConditionExpression(TenderFields.EmployerId, ConditionOperator.NotNull),
                new ConditionExpression(TenderFields.IsEOI, ConditionOperator.Equal, TenderState.TENDER_CRM_VALUE),
                new ConditionExpression(TenderFields.IsClosed, ConditionOperator.Equal, "0"),
                new ConditionExpression(TenderFields.ClosingDate, ConditionOperator.GreaterThan, DateTime.Now)
            });

            ct.ThrowIfCancellationRequested();

            var entities = _service.RetrieveMultiple(query).Entities;

            ct.ThrowIfCancellationRequested();

            var grouped = entities
                .Where(x =>
                    x.Contains(TenderFields.ClassOfWork) &&
                    x.Contains(TenderFields.TenderValueRange))
                .GroupBy(x => new
                {
                    CoW = x.GetAttributeValue<EntityReference>(TenderFields.ClassOfWork),
                    Grade = x.GetAttributeValue<OptionSetValue>(TenderFields.TenderValueRange)?.Value
                })
                .Where(g => g.Key.CoW != null);

            var rows = grouped
                .GroupBy(g => g.Key.CoW.Id)
                .Select(g =>
                {
                    var first = g.First();

                    return new ClassOfWorkSummary
                    {
                        ClassOfWorkId = first.Key.CoW.Id,
                        ClassOfWorkName = first.Key.CoW.Name,
                        DesignationIdCounts = g.ToDictionary(
                            x => x.Key.Grade ?? 0,
                            x => x.Count()
                        )
                    };
                })
                .ToList();

            result.Rows = rows;

            return result;
        }

        public Task<Guid> CreateAsync(CreateTenderModel model, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var tender = new Entity(CrmEntityNames.Tender);

            #region Notice Details

            tender[TenderFields.EmployerTenderNumber] = model.EmployerTenderNumber;
            tender[TenderFields.Title] = model.Title;
            tender[TenderFields.Name] = model.Name;
            tender[TenderFields.TendersInvitedFor] = model.TendersInvitedFor;
            tender[TenderFields.PreferencesOffered] = model.PreferencesOffered;
            tender[TenderFields.EligibilityCriteria] = model.EligibilityCriteria;

            #endregion

            #region Location Details

            if (model.ProvinceId.HasValue)
                tender[TenderFields.ProvinceId] =
                    new EntityReference(CrmEntityNames.Province, model.ProvinceId.Value);

            if (model.MetroDistrictId.HasValue)
                tender[TenderFields.MetroDistrictId] =
                    new EntityReference(CrmEntityNames.MetroDistrict, model.MetroDistrictId.Value);

            if (model.LocalMunicipalityId.HasValue)
                tender[TenderFields.LocalMunicipalityId] =
                    new EntityReference(TenderFields.LocalMunicipalityId, model.LocalMunicipalityId.Value);

            #endregion

            #region Contract Details

            if (model.TypeOfContractId.HasValue)
            {
                tender[TenderFields.TypeOfContract] =
                    new OptionSetValue(model.TypeOfContractId.Value);
            }

            if (model.ClassOfConstructionWorksId.HasValue)
                tender[TenderFields.ClassOfWork] =
                    new EntityReference(
                        CrmEntityNames.ClassOfWorkType,
                        model.ClassOfConstructionWorksId.Value);

            if (model.SubCategoryId.HasValue)
                tender[TenderFields.SubCategory] =
                    new EntityReference(
                        CrmEntityNames.ClassOfWorkTypeSub,
                        model.SubCategoryId.Value);

            if (model.AlternateClassOfConstructionWorksId.HasValue)
                tender[TenderFields.AltClassOfWork] =
                    new EntityReference(
                        TenderFields.AltClassOfWork,
                        model.AlternateClassOfConstructionWorksId.Value);

            if (model.TenderValueRangeId.HasValue)
                tender[TenderFields.TenderValueRange] =
                    new OptionSetValue(model.TenderValueRangeId.Value);

            tender[TenderFields.EmergingEnterpriseSupport] =
                new OptionSetValue(model.EmergingEnterpriseSupportId.Value);

            //tender[TenderFields.ExpandedPublicWorksProgram] =
            //    new OptionSetValue(model.ExpandedPublicWorksProgramId);

            tender[TenderFields.NameOfTargetedDevelopmentProgramme] =
                model.NameOfTargetedDevelopmentProgramme;

            tender[TenderFields.NationalContractorDevelopmentProgramme] =
                new OptionSetValue(model.NationalContractorDevelopmentProgrammeId);

            tender[TenderFields.IsTermContract] = new OptionSetValue(model.IsTermContract.Value);

            if (model.Status == TenderStatus.Draft)
            {
                tender[TenderFields.StatusCode] = "1";
            }
            if (model.Status == TenderStatus.Advertised)
            {
                tender[TenderFields.StatusCode] = "100000000";
            }
            if (model.Status == TenderStatus.Cancelled)
            {
                tender[TenderFields.StatusCode] = "100000001";
            }

            #endregion

            #region Employer Details

            if (model.EmployerId.HasValue)
                tender[TenderFields.EmployerId] =  new EntityReference(CrmEntityNames.Account, model.EmployerId.Value);

            tender[TenderFields.DocsAvailableFrom] =
                model.DocumentsAvailableFrom;

            tender[TenderFields.DepositAmount] =
                model.DepositAmount;

            tender[TenderFields.MethodOfPaymentCash] = model.MethodOfPaymentCash;

            tender[TenderFields.MethodOfPaymentBankGuaranteedCheque] = model.MethodOfPaymentBankGuaranteedCheque;

            tender[TenderFields.MethodOfPaymentProofOfDeposit] = model.MethodOfPaymentProofOfDepost;

            tender[TenderFields.FurtherPaymentAndCollectionInformation] =
                model.FurtherPaymentAndCollectionInformation;

            #endregion

            #region Address

            if (model.PrimaryAddress != null)
            {
                tender[TenderFields.PrimaryAddressLine1] = model.PrimaryAddress.Line1;
                tender[TenderFields.PrimaryAddressLine2] = model.PrimaryAddress.City;
                tender[TenderFields.PrimaryAddressLine3] = model.PrimaryAddress.Province;
                tender[TenderFields.PrimaryAddressLine4] = model.PrimaryAddress.PostalCode;
            }

            if (model.AdditionalCollectionAddress != null &&
                (!string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.Line1) ||
                 !string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.City) ||
                 !string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.Province) ||
                 !string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.PostalCode)))
                {
                    tender[TenderFields.SecondaryAddressLine1] = model.AdditionalCollectionAddress.Line1;
                    tender[TenderFields.SecondaryAddressLine2] = model.AdditionalCollectionAddress.City;
                    tender[TenderFields.SecondaryAddressLine3] = model.AdditionalCollectionAddress.Province;
                    tender[TenderFields.SecondaryAddressLine4] = model.AdditionalCollectionAddress.PostalCode;
                }

            #endregion

            #region Clarification Meeting

            tender[TenderFields.ClarificationMeeting] = new OptionSetValue(model.ClarificationMeetingRequired);

            tender[TenderFields.ClarificationMeetingPlace] =
                model.ClarificationMeetingPlace;

            tender[TenderFields.ClarificationMeetingDateAndTime] =
                model.ClarificationMeetingDateAndTime;

            tender[TenderFields.ClarificationMeetingCompulsory] = new OptionSetValue(model.ClarificationMeetingCompulsory.Value);

            tender[TenderFields.AdditionalClarificationMeeting] = new OptionSetValue(model.AdditionalClarificationMeeting.Value);

            tender[TenderFields.AdditionalClarificationMeetingPlace] =
                model.AddClarificationMeetingPlace;

            tender[TenderFields.AdditionalClarificationMeetingDateAndTime] =
                model.AddClarificationMeetingDateAndTime;

            tender[TenderFields.AdditionalClarificationMeetingCompulsory] = new OptionSetValue(model.AddClarificationMeetingCompulsory.Value);

            #endregion

            #region Submission Details

            tender[TenderFields.ClosingDate] =
                model.ClosingDateTime;

            tender[TenderFields.NotAcceptFacsimile] =
                model.NotAcceptedEmail;

            //tender[TenderFields.DateAdvertised] = DateTime.UtcNow;
            tender[TenderFields.IsClosed] = false;

            #endregion

            ct.ThrowIfCancellationRequested();

            var tenderId = _service.Create(tender);

            ct.ThrowIfCancellationRequested();

            return Task.FromResult(tenderId);
        }

        public async Task<TenderModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            try
            {
                var entity = await Task.Run(() =>
                    _service.Retrieve(
                        CrmEntityNames.Tender,
                        id,
                        new ColumnSet(true)
                    ), ct);

                return TenderMapper.ToDomain(entity);
            }
            catch (FaultException<OrganizationServiceFault> ex)
                when (ex.Detail.ErrorCode == unchecked((int)0x80040217))
            {
                // Record not found
                return null;
            }
        }

        public Task<Guid> ChangeTenderStatusAsync(TenderModel model, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id == Guid.Empty)
                throw new ArgumentException("Tender Id is required for update.");

            var tender = new Entity(CrmEntityNames.Tender)
            {
                Id = model.Id
            };

            tender[TenderFields.StatusCode] = new OptionSetValue(model.StatusCodeId.Value);

            ct.ThrowIfCancellationRequested();

            _service.Update(tender);
            return Task.FromResult(model.Id);
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.");

            // Optional: check existence first (recommended in real systems)
            _service.Delete(CrmEntityNames.Tender, id);

            return Task.CompletedTask;
        }

        public Task<Guid> UpdateAsync(UpdateTenderModel model, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id == Guid.Empty)
                throw new ArgumentException("Tender Id is required.", nameof(model.Id));

            var tender = new Entity(CrmEntityNames.Tender)
            {
                Id = model.Id
            };

            #region Notice Details

            if (!string.IsNullOrWhiteSpace(model.Title))
                tender[TenderFields.Title] = model.Title;

            if (!string.IsNullOrWhiteSpace(model.Name))
                tender[TenderFields.Name] = model.Name;

            if (!string.IsNullOrWhiteSpace(model.TendersInvitedFor))
                tender[TenderFields.TendersInvitedFor] = model.TendersInvitedFor;

            if (!string.IsNullOrWhiteSpace(model.PreferencesOffered))
                tender[TenderFields.PreferencesOffered] = model.PreferencesOffered;

            if (!string.IsNullOrWhiteSpace(model.EligibilityCriteria))
                tender[TenderFields.EligibilityCriteria] = model.EligibilityCriteria;

            #endregion

            #region Location Details

            if (model.ProvinceId.HasValue)
                tender[TenderFields.ProvinceId] =
                    new EntityReference(CrmEntityNames.Province, model.ProvinceId.Value);

            if (model.MetroDistrictId.HasValue)
                tender[TenderFields.MetroDistrictId] =
                    new EntityReference(CrmEntityNames.MetroDistrict, model.MetroDistrictId.Value);

            if (model.LocalMunicipalityId.HasValue)
                tender[TenderFields.LocalMunicipalityId] =
                    new EntityReference(
                        TenderFields.LocalMunicipalityId,
                        model.LocalMunicipalityId.Value);

            #endregion

            #region Contract Details

            if (model.TypeOfContractId.HasValue)
                tender[TenderFields.TypeOfContract] =
                    new OptionSetValue(model.TypeOfContractId.Value);

            if (model.ClassOfConstructionWorksId.HasValue)
                tender[TenderFields.ClassOfWork] =
                    new EntityReference(
                        CrmEntityNames.ClassOfWorkType,
                        model.ClassOfConstructionWorksId.Value);

            if (model.SubCategoryId.HasValue)
                tender[TenderFields.SubCategory] =
                    new EntityReference(
                        CrmEntityNames.ClassOfWorkTypeSub,
                        model.SubCategoryId.Value);

            if (model.AlternateClassOfConstructionWorksId.HasValue)
                tender[TenderFields.AltClassOfWork] =
                    new EntityReference(
                        CrmEntityNames.ClassOfWorkType,
                        model.AlternateClassOfConstructionWorksId.Value);

            if (model.TenderValueRangeId.HasValue)
                tender[TenderFields.TenderValueRange] =
                    new OptionSetValue(model.TenderValueRangeId.Value);

            if (model.EmergingEnterpriseSupportId.HasValue)
                tender[TenderFields.EmergingEnterpriseSupport] =
                    new OptionSetValue(model.EmergingEnterpriseSupportId.Value);

            if (!string.IsNullOrWhiteSpace(model.NameOfTargetedDevelopmentProgramme))
                tender[TenderFields.NameOfTargetedDevelopmentProgramme] =
                    model.NameOfTargetedDevelopmentProgramme;

            if (model.NationalContractorDevelopmentProgrammeId.HasValue)
                tender[TenderFields.NationalContractorDevelopmentProgramme] =
                    new OptionSetValue(model.NationalContractorDevelopmentProgrammeId.Value);

            if (model.IsTermContract.HasValue)
                tender[TenderFields.IsTermContract] =
                    new OptionSetValue(model.IsTermContract.Value);

            #endregion

            #region Employer Details

            if (model.DocumentsAvailableFrom.HasValue)
                tender[TenderFields.DocsAvailableFrom] =
                    model.DocumentsAvailableFrom.Value;

            if (model.DepositAmount.HasValue)
                tender[TenderFields.DepositAmount] =
                    model.DepositAmount.Value;

            if (model.MethodOfPaymentCash.HasValue)
                tender[TenderFields.MethodOfPaymentCash] =
                    model.MethodOfPaymentCash.Value;

            if (model.MethodOfPaymentBankGuaranteedCheque.HasValue)
                tender[TenderFields.MethodOfPaymentBankGuaranteedCheque] =
                    model.MethodOfPaymentBankGuaranteedCheque.Value;

            if (model.MethodOfPaymentProofOfDepost.HasValue)
                tender[TenderFields.MethodOfPaymentProofOfDeposit] =
                    model.MethodOfPaymentProofOfDepost.Value;

            if (!string.IsNullOrWhiteSpace(model.FurtherPaymentAndCollectionInformation))
                tender[TenderFields.FurtherPaymentAndCollectionInformation] =
                    model.FurtherPaymentAndCollectionInformation;

            #endregion

            #region Address

            if (model.PrimaryAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(model.PrimaryAddress.Line1))
                    tender[TenderFields.PrimaryAddressLine1] =
                        model.PrimaryAddress.Line1;

                if (!string.IsNullOrWhiteSpace(model.PrimaryAddress.City))
                    tender[TenderFields.PrimaryAddressLine2] =
                        model.PrimaryAddress.City;

                if (!string.IsNullOrWhiteSpace(model.PrimaryAddress.Province))
                    tender[TenderFields.PrimaryAddressLine3] =
                        model.PrimaryAddress.Province;

                if (!string.IsNullOrWhiteSpace(model.PrimaryAddress.PostalCode))
                    tender[TenderFields.PrimaryAddressLine4] =
                        model.PrimaryAddress.PostalCode;
            }

            if (model.AdditionalCollectionAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.Line1))
                    tender[TenderFields.SecondaryAddressLine1] =
                        model.AdditionalCollectionAddress.Line1;

                if (!string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.City))
                    tender[TenderFields.SecondaryAddressLine2] =
                        model.AdditionalCollectionAddress.City;

                if (!string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.Province))
                    tender[TenderFields.SecondaryAddressLine3] =
                        model.AdditionalCollectionAddress.Province;

                if (!string.IsNullOrWhiteSpace(model.AdditionalCollectionAddress.PostalCode))
                    tender[TenderFields.SecondaryAddressLine4] =
                        model.AdditionalCollectionAddress.PostalCode;
            }

            #endregion

            #region Clarification Meeting

            if (model.ClarificationMeetingRequired.HasValue)
                tender[TenderFields.ClarificationMeeting] =
                    new OptionSetValue(model.ClarificationMeetingRequired.Value);

            if (!string.IsNullOrWhiteSpace(model.ClarificationMeetingPlace))
                tender[TenderFields.ClarificationMeetingPlace] =
                    model.ClarificationMeetingPlace;

            if (model.ClarificationMeetingDateAndTime.HasValue)
                tender[TenderFields.ClarificationMeetingDateAndTime] =
                    model.ClarificationMeetingDateAndTime.Value;

            if (model.ClarificationMeetingCompulsory.HasValue)
                tender[TenderFields.ClarificationMeetingCompulsory] =
                    new OptionSetValue(model.ClarificationMeetingCompulsory.Value);

            if (model.AdditionalClarificationMeeting.HasValue)
                tender[TenderFields.AdditionalClarificationMeeting] =
                    new OptionSetValue(model.AdditionalClarificationMeeting.Value);

            if (!string.IsNullOrWhiteSpace(model.AddClarificationMeetingPlace))
                tender[TenderFields.AdditionalClarificationMeetingPlace] =
                    model.AddClarificationMeetingPlace;

            if (model.AddClarificationMeetingDateAndTime.HasValue)
                tender[TenderFields.AdditionalClarificationMeetingDateAndTime] =
                    model.AddClarificationMeetingDateAndTime.Value;

            if (model.AddClarificationMeetingCompulsory.HasValue)
                tender[TenderFields.AdditionalClarificationMeetingCompulsory] =
                    new OptionSetValue(model.AddClarificationMeetingCompulsory.Value);

            #endregion

            #region Submission Details

            if (model.ClosingDateTime.HasValue)
                tender[TenderFields.ClosingDate] =
                    model.ClosingDateTime.Value;

            if (model.NotAcceptedEmail.HasValue)
                tender[TenderFields.NotAcceptFacsimile] =
                    model.NotAcceptedEmail.Value;

            #endregion

            ct.ThrowIfCancellationRequested();

            if (tender.Attributes.Count == 0)
                return Task.FromResult(model.Id);

            _service.Update(tender);

            ct.ThrowIfCancellationRequested();

            return Task.FromResult(model.Id);
        }

        public List<TenderModel> GetTendersAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var query = new QueryExpression(CrmEntityNames.Tender)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And),
                PageInfo = new PagingInfo
                {
                    ReturnTotalRecordCount = true,
                    PageNumber = 1,
                    Count = 5000
                }
            };

            query.Criteria.Conditions.AddRange(new List<ConditionExpression>
            {
                new ConditionExpression(TenderFields.StateCode, ConditionOperator.NotEqual, StateCodes.StateCode_Inactive),
                new ConditionExpression(TenderFields.StatusCode, ConditionOperator.Equal, TenderState.ADVERTISED_STATUS),
                new ConditionExpression(TenderFields.DateAdvertised, ConditionOperator.NotNull),
                new ConditionExpression(TenderFields.EmployerId, ConditionOperator.NotNull),
                new ConditionExpression(TenderFields.IsEOI, ConditionOperator.Equal, TenderState.TENDER_CRM_VALUE),
                new ConditionExpression(TenderFields.IsClosed, ConditionOperator.Equal, "0"),
                new ConditionExpression(TenderFields.ClosingDate, ConditionOperator.GreaterThan, DateTime.UtcNow)
            });

            var result = _service.RetrieveMultiple(query);

            return result.Entities
                .Select(TenderMapper.ToDomain)
                .ToList();
        }
    }
}
