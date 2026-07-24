using iTender.WorkerService.Models;

namespace iTender.WorkerService.Providers
{
    public interface IETenderApiProvider
    {
        Task<List<ExternalTender>> GetTendersAsync(CancellationToken ct);
    }
}
