using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<AccountModel?> GetByCRSNumberAsync(string crsNumber, CancellationToken ct = default);
        Task<IReadOnlyList<AccountModel>> SearchAsync(string searchText, CancellationToken ct = default);
        Task<IReadOnlyList<AccountModel>> GetSanctionedAccountsAsync(CancellationToken ct = default);
        Task<Guid> CreateAsync(AccountModel model, CancellationToken ct = default);
        Task UpdateAsync(AccountModel model, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
