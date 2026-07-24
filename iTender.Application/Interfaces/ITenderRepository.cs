using iTender.Application.DTOs;
using iTender.Domain.Enums;
using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface ITenderRepository
    {
        Task<Guid> CreateAsync(CreateTenderModel model, CancellationToken ct = default);
        Task<TenderModel> GetByIdAsync(Guid id, CancellationToken ct = default);
        List<TenderModel> GetTendersAsync(CancellationToken ct = default);
        Task<Guid> ChangeTenderStatusAsync(TenderModel model, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        PagedResult<TenderModel> GetByFilter(TenderFilterModel? filter, CancellationToken ct = default);
        TenderSummaryViewModel GetTenderSummary(CancellationToken ct = default);
        PagedResult<TenderModel> GetAdvancedFilteredTenders(AdvancedTenderSearchViewModel model, CancellationToken ct = default);
        List<ProvinceStatViewModel> GetTenderCountByProvince(CancellationToken ct = default);
        Task<PagedResult<TenderModel>> GetTenders(TenderFilterViewModel filter, Guid employerId, TenderType tenderType, CancellationToken ct = default);
        Task<Guid> UpdateAsync(UpdateTenderModel model, CancellationToken ct = default);
    }
}
