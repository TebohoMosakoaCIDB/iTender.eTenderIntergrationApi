using iTender.Application.DTOs;

namespace iTender.Application.Providers
{
    public interface IETenderApiProvider
    {
        Task<List<ExternalTenderModel>> GetTendersAsync(CancellationToken ct = default);
    }
}
