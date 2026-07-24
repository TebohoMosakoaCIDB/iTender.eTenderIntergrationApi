using iTender.Application.DTOs;
using iTender.Application.Providers;
using System.Net.Http.Json;


namespace iTender.Infrastructure.Providers
{
    public class ETenderApiProvider : IETenderApiProvider
    {
        private readonly HttpClient _httpClient;

        public ETenderApiProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ExternalTenderModel>> GetTendersAsync(CancellationToken ct = default)
        {
            var response = await _httpClient.GetAsync("Tenders", ct);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<ExternalTenderModel>>(cancellationToken: ct)
                   ?? new();
        }
    }
}
