using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace iTender.Infrastructure.Repositories
{
    public class LookupService : ILookupService
    {
        private readonly IOrganizationService _service;

        public LookupService(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }

        #region City
        public async Task<PagedResult<CityModel>> GetAllCitiesAsync(PagedRequest request, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.City)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true,
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(CityMapper.ToDomain)
                .ToList();

            return new PagedResult<CityModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = response.TotalRecordCount
            };
        }

        public Task<CityModel?> GetCityByIdAsync(Guid id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var entity = _service.Retrieve(
                CrmEntityNames.City,
                id,
                new ColumnSet(true));

            return Task.FromResult(
                CityMapper.ToDomain(entity));
        }

        public async Task<PagedResult<CityModel>> GetCitiesByProvinceIdAsync(Guid provinceId, PagedRequest request, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.City)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true,
                Criteria =
        {
            Conditions =
            {
                new ConditionExpression(CrmFieldNames.CityFields.ProvinceId, ConditionOperator.Equal, provinceId)
            }
        },
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(CityMapper.ToDomain)
                .ToList();

            return new PagedResult<CityModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = -1
            };
        }

        #endregion

        #region Class Of Construction Work
        public async Task<PagedResult<ClassOfConstructionWorkModel>> GetAllClassOfConstructionWorksAsync(PagedRequest request, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.ClassOfWorkType)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true,
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(ClassOfConstructionWorkMapper.ToDomain)
                .ToList();

            return new PagedResult<ClassOfConstructionWorkModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = response.TotalRecordCount
            };
        }

        public async Task<ClassOfConstructionWorkModel?> GetClassOfConstructionWorkByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return null;

            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.ClassOfWorkType,
                    id,
                    new ColumnSet(true)), ct);

            if (entity == null)
                return null;

            return ClassOfConstructionWorkMapper.ToDomain(entity);
        }

        public async Task<ClassOfConstructionWorkModel?> GetClassOfConstructionWorkByNameAsync(string name, string[]? columns = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = new QueryExpression(CrmEntityNames.ClassOfWorkType)
            {
                ColumnSet = columns != null && columns.Length > 0
                    ? new ColumnSet(columns)
                    : new ColumnSet(true),

                TopCount = 1,
                NoLock = true
            };

            query.Criteria.AddCondition(
                CrmFieldNames.ClassOfConstructionWorkFields.Name,
                ConditionOperator.Contains,
                name.Trim()
            );

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var entity = response.Entities.FirstOrDefault();

            if (entity == null)
                return null;

            return ClassOfConstructionWorkMapper.ToDomain(entity);
        }
        #endregion

        #region Class Of Work Type Sub Category
        public async Task<PagedResult<ClassOfWorkTypeSubModel>> GetAllClassOfWorkTypeSubCategoriesAsync(PagedRequest request, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.ClassOfWorkTypeSub)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true,
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(ClassOfWorkTypeSubMapper.ToDomain)
                .ToList();

            return new PagedResult<ClassOfWorkTypeSubModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = response.TotalRecordCount
            };
        }

        public async Task<PagedResult<ClassOfWorkTypeSubModel>> GetClassOfWorkTypeSubCategoriesByWorkTypeIdAsync(Guid classOfWorkTypeId, PagedRequest request, string[]? columns = null, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.ClassOfWorkTypeSub)
            {
                ColumnSet = columns != null && columns.Length > 0
                    ? new ColumnSet(columns)
                    : new ColumnSet(true),

                NoLock = true,
                Criteria =
                    {
                    Conditions =
                        {
                            new ConditionExpression(
                                "classofworktypeid",
                                ConditionOperator.Equal,
                                classOfWorkTypeId)
                        }
                    },
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(ClassOfWorkTypeSubMapper.ToDomain)
                .ToList();

            return new PagedResult<ClassOfWorkTypeSubModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = response.TotalRecordCount
            };
        }

        public async Task<ClassOfWorkTypeSubModel?> GetClassOfWorkTypeSubCategoryByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return null;

            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.ClassOfWorkTypeSub,
                    id,
                    new ColumnSet(true)), ct);

            if (entity == null)
                return null;

            return ClassOfWorkTypeSubMapper.ToDomain(entity);
        }

        #endregion

        #region Metro District
        public async Task<PagedResult<MetroDistrictModel>> GetAllMetroDistrictsAsync(PagedRequest request, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.MetroDistrict)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true,
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(MetroDistrictMapper.ToDomain)
                .ToList();

            return new PagedResult<MetroDistrictModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = response.TotalRecordCount
            };
        }

        public async Task<MetroDistrictModel?> GetMetroDistrictByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                return null;

            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.MetroDistrict,
                    id,
                    new ColumnSet(true)), ct);

            if (entity == null)
                return null;

            return MetroDistrictMapper.ToDomain(entity);
        }

        public async Task<PagedResult<MetroDistrictModel>> GetMetroDistrictsByProvinceIdAsync(Guid provinceId, PagedRequest request, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.MetroDistrict)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true,
                Criteria =
            {
                Conditions =
                {
                    new ConditionExpression(
                        CrmFieldNames.MetroDistrictFields.ProvinceId,
                        ConditionOperator.Equal,
                        provinceId)
                }
            },
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(MetroDistrictMapper.ToDomain)
                .ToList();

            return new PagedResult<MetroDistrictModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = response.TotalRecordCount
            };
        }
        #endregion

        #region Provinces
        public async Task<ProvinceModel?> GetProvinceByIdAsync(Guid provinceId, CancellationToken ct = default)
        {
            if (provinceId == Guid.Empty)
                return null;

            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.Province,
                    provinceId,
                    new ColumnSet(true)), ct);

            if (entity == null)
                return null;

            return ProvinceMapper.ToDomain(entity);
        }

        public async Task<PagedResult<ProvinceModel>> GetAllProvinces(PagedRequest request, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Province)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true,
                PageInfo = new PagingInfo
                {
                    PageNumber = request.PageNumber,
                    Count = request.PageSize,
                    ReturnTotalRecordCount = true
                }
            };

            var response = await Task.Run(() =>
                _service.RetrieveMultiple(query), ct);

            var items = response.Entities
                .Select(ProvinceMapper.ToDomain)
                .ToList();

            return new PagedResult<ProvinceModel>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = response.TotalRecordCount
            };
        }
        #endregion

        #region Tender Value Range
        public async Task<TenderValueRangeModel> GetTenderValueRangeById(int id, CancellationToken ct = default)
        {
            var all = await GetAllTenderValueRange(ct);
            var item = all.FirstOrDefault(x => x.Id == id);

            return item != null ? item : new TenderValueRangeModel();
        }

        public Task<List<TenderValueRangeModel>> GetAllTenderValueRange(CancellationToken ct = default)
        {
            var request = new RetrieveAttributeRequest
            {
                EntityLogicalName = CrmEntityNames.Contract,
                LogicalName = CrmEntityNames.TenderValueRange,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveAttributeResponse)_service.Execute(request);
            var metadata = response.AttributeMetadata as EnumAttributeMetadata;

            var result = metadata?.OptionSet?.Options?
                .Select(o => new TenderValueRangeModel
                {
                    Id = o.Value ?? 0,
                    Name = o.Label?.UserLocalizedLabel?.Label ?? string.Empty
                })
                .ToList()
                ?? new List<TenderValueRangeModel>();

            return Task.FromResult(result);
        }

        public async Task<int?> ResolveApprovedGradeFromTenderRangeAsync(int tenderRangeId, CancellationToken ct = default)
        {
            var tenderRanges = await GetAllTenderValueRange(ct);

            var selected = tenderRanges
                .FirstOrDefault(x => x.Id == tenderRangeId);

            if (selected == null || string.IsNullOrWhiteSpace(selected.Name))
                return null;

            // Example: "1 - R200 000" -> grade 1
            var firstChar = selected.Name.Substring(0, 1);

            if (int.TryParse(firstChar, out var grade))
                return grade;

            return null;
        }
        #endregion

        #region Type Of Contract
        public async Task<TypeOfContractModel> GetTypeOfContractById(int id, CancellationToken ct = default)
        {
            var all = await GetAllTypeOfContracts(ct);
            var item = all.FirstOrDefault(x => x.Id == id);

            return item != null ? item : new TypeOfContractModel();
        }

        public Task<List<TypeOfContractModel>> GetAllTypeOfContracts(CancellationToken ct = default)
        {
            var request = new RetrieveAttributeRequest
            {
                EntityLogicalName = CrmEntityNames.Contract,
                LogicalName = CrmEntityNames.TypeOfContract,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveAttributeResponse)_service.Execute(request);
            var metadata = response.AttributeMetadata as EnumAttributeMetadata;

            var result = metadata?.OptionSet?.Options?
                .Select(o => new TypeOfContractModel
                {
                    Id = o.Value ?? 0,
                    Name = o.Label?.UserLocalizedLabel?.Label ?? string.Empty
                })
                .ToList()
                ?? new List<TypeOfContractModel>();

            return Task.FromResult(result);
        }
        #endregion
    }
}
