using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface ICredentialRepository
    {
        Task<CredentialModel?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<CredentialModel?> GetByUsernameAsync(string username, CancellationToken ct = default);

        Task<CredentialModel?> GetByCredentialsAsync(string username, string password, CancellationToken ct = default);

        //Task<Guid> CreateAsync(CredentialModel model, CancellationToken ct = default);

        Task UpdateAsync(CredentialModel model, CancellationToken ct = default);
    }
}
