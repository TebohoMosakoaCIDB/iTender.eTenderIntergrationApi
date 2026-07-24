using iTender.Domain.Models;

namespace iTender.WorkerService.Providers
{
    public interface IiTenderApiProvider
    {
        Task<List<TenderModel>> GetAllAsync(CancellationToken ct);
    }
}
