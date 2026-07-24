using iTender.Application.DTOs;
using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface ILookupService
    {
        #region Metro District

        Task<PagedResult<MetroDistrictModel>> GetAllMetroDistrictsAsync(
            PagedRequest request,
            CancellationToken ct = default);

        Task<MetroDistrictModel?> GetMetroDistrictByIdAsync(
            Guid id,
            CancellationToken ct = default);

        Task<PagedResult<MetroDistrictModel>> GetMetroDistrictsByProvinceIdAsync(
            Guid provinceId,
            PagedRequest request,
            CancellationToken ct = default);

        #endregion

        #region Province
        Task<ProvinceModel?> GetProvinceByIdAsync(Guid provinceId, CancellationToken ct = default);
        Task<PagedResult<ProvinceModel>> GetAllProvinces(PagedRequest request, CancellationToken ct = default);
        #endregion

        #region Tender Value Range
        Task<TenderValueRangeModel> GetTenderValueRangeById(int id, CancellationToken ct = default);
        Task<List<TenderValueRangeModel>> GetAllTenderValueRange(CancellationToken ct = default);
        Task<int?> ResolveApprovedGradeFromTenderRangeAsync(int tenderRangeId, CancellationToken ct = default);
        #endregion

        #region City

        Task<PagedResult<CityModel>> GetAllCitiesAsync(
            PagedRequest request,
            CancellationToken ct = default);

        Task<CityModel?> GetCityByIdAsync(
            Guid id,
            CancellationToken ct = default);

        Task<PagedResult<CityModel>> GetCitiesByProvinceIdAsync(
            Guid provinceId,
            PagedRequest request,
            CancellationToken ct = default);

        #endregion

        #region Class Of Construction Work

        Task<PagedResult<ClassOfConstructionWorkModel>> GetAllClassOfConstructionWorksAsync(
            PagedRequest request,
            CancellationToken ct = default);

        Task<ClassOfConstructionWorkModel?> GetClassOfConstructionWorkByIdAsync(
            Guid id,
            CancellationToken ct = default);

        Task<ClassOfConstructionWorkModel?> GetClassOfConstructionWorkByNameAsync(
            string name,
            string[]? columns = null,
            CancellationToken ct = default);


        #endregion

        #region Class Of Work Type Sub Category

        Task<PagedResult<ClassOfWorkTypeSubModel>> GetAllClassOfWorkTypeSubCategoriesAsync(
            PagedRequest request,
            CancellationToken ct = default);

        Task<ClassOfWorkTypeSubModel?> GetClassOfWorkTypeSubCategoryByIdAsync(
            Guid id,
            CancellationToken ct = default);

        Task<PagedResult<ClassOfWorkTypeSubModel>> GetClassOfWorkTypeSubCategoriesByWorkTypeIdAsync(
            Guid classOfWorkTypeId,
            PagedRequest request,
            string[]? columns = null,
            CancellationToken ct = default);

        #endregion

        #region Type Of Contract
        Task<TypeOfContractModel> GetTypeOfContractById(int id, CancellationToken ct = default);
        Task<List<TypeOfContractModel>> GetAllTypeOfContracts(CancellationToken ct = default);
        #endregion
    }
}
