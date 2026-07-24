using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface ICdpSubmissionRepository
    {
        Task<List<CdpSubmissionModel>> GetByCdpIdAsync(Guid cdpId, CancellationToken ct);
    }
}
