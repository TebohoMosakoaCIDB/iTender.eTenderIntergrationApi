using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IApplicationRepository
    {
        Task<ApplicationModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<ApplicationModel?> GetByApplicationNumberAsync(string applicationNumber, CancellationToken ct = default);
        Task<IEnumerable<ApplicationModel>> SearchAsync(string searchText, CancellationToken ct = default);
        Task<Guid> CreateAsync(ApplicationModel model, CancellationToken ct = default);
        Task UpdateAsync(ApplicationModel model, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
