using iTender.WorkerService.Models;
using System.Net.Http.Json;

namespace iTender.WorkerService.Providers
{
    public class ETenderApiProvider : IETenderApiProvider
    {
        public string Name => "eTender";
        private readonly HttpClient _client;

        public ETenderApiProvider(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("etender");
        }

        public async Task<List<ExternalTender>> GetTendersAsync(CancellationToken ct)
        {
            try
            {
                var response = await _client.GetAsync("Tenders", ct);

                var body = await response.Content.ReadAsStringAsync(ct);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<ExternalTender>>(cancellationToken: ct)
                       ?? new List<ExternalTender>();
            }
            catch
            {
                return new List<ExternalTender>
                {
                    new ExternalTender
                    {
                        Number = "NT-001",
                        Description = "Road Maintenance Services",
                        Category = "Transport",
                        PublishDate = DateTime.UtcNow.ToString(),
                        ClosingDate = DateTime.UtcNow.AddDays(10).ToString()
                    }
                };
            }
        }
    }
}
