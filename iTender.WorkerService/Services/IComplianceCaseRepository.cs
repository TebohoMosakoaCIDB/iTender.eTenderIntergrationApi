using iTender.WorkerService.Models;

namespace iTender.WorkerService.Services
{
    public interface IComplianceCaseRepository
    {
        Task<List<ComplianceCase>> GetOpenCasesAsync(CancellationToken ct);
        Task<ComplianceCase?> GetByTenderNumberAsync(string tenderNumber, CancellationToken ct);
        Task CreateAsync(ComplianceCase caseItem, CancellationToken ct);
        Task UpdateAsync(ComplianceCase caseItem, CancellationToken ct);
    }
}
