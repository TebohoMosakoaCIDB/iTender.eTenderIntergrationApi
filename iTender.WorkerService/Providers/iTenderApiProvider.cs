using iTender.Domain.Models;
using System.Net.Http.Json;

namespace iTender.WorkerService.Providers
{
    public class iTenderApiProvider : IiTenderApiProvider
    {
        private readonly HttpClient _client;

        public string Name => "iTender";

        public iTenderApiProvider(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("itender-api");
        }

        public async Task<List<TenderModel>> GetAllAsync(CancellationToken ct)
        {
            return await _client.GetFromJsonAsync<List<TenderModel>>("", ct)
                   ?? new();
        }
    }
}
