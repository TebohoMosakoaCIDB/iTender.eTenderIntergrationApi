using iTender.WorkerService.Models;

namespace iTender.WorkerService.Services
{
    public interface IComplianceCaseService
    {
        Task ProcessNonCompliantTendersAsync(List<ClassifiedTender> nonCompliantTenders, CancellationToken ct);
    }
}
