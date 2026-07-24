using iTender.Domain.Models;
using iTender.WorkerService.Models;
using iTender.WorkerService.Providers;

namespace iTender.WorkerService.Services
{
    public class TenderProviderService
    {
        private readonly IEnumerable<IETenderApiProvider> _providers;
        private readonly IEnumerable<IiTenderApiProvider> _internalProvider;
        private readonly ILogger<TenderProviderService> _logger;

        public TenderProviderService(
            IEnumerable<IiTenderApiProvider> internalProvider,
            IEnumerable<IETenderApiProvider> providers,
            ILogger<TenderProviderService> logger)
        {
            _internalProvider = internalProvider;
            _providers = providers;
            _logger = logger;
        }

        public async Task<List<ExternalTender>> GetAllAsync(CancellationToken ct)
        {
            var all = new List<ExternalTender>();

            foreach (var provider in _providers)
            {
                try
                {
                    _logger.LogInformation("Fetching from eTender");

                    var result = await provider.GetTendersAsync(ct);

                    _logger.LogInformation("eTender returned {count} tenders", result.Count);

                    all.AddRange(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Provider failed: eTender");
                }
            }

            return all;
        }

        public async Task<List<TenderModel>> GetiTenderTendersAsync(CancellationToken ct)
        {
            var all = new List<TenderModel>();

            foreach (var provider in _internalProvider)
            {
                try
                {
                    _logger.LogInformation("Fetching from iTender");

                    var result = await provider.GetAllAsync(ct);

                    _logger.LogInformation("iTender returned {count} tenders", result.Count);

                    all.AddRange(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Provider failed: iTender");
                }
            }

            return all;
        }
    }
}
