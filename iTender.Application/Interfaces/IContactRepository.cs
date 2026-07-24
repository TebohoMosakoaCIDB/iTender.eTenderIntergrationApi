using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IContactRepository
    {
        Task<ContactModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(ContactModel model, CancellationToken ct = default);
        Task UpdateAsync(ContactModel model, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        List<ContactModel> GetUserByEmailAddress(string emailAddress, string[] columns);
        Task<List<ContactModel>> GetContactsByTenderId(Guid Id, CancellationToken ct = default);
        Task<List<PermissionModel>> GetContactsPermissions(Guid Id, CancellationToken ct = default);
    }
}
