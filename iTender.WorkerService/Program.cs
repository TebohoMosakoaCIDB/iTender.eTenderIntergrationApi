using iTender.WorkerService;
using iTender.WorkerService.Options;
using iTender.WorkerService.Providers;
using iTender.WorkerService.Services;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net.Http.Headers;
using System.Text;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// CONFIG FIRST
builder.Services.Configure<InternalApiOptions>(
    builder.Configuration.GetSection("InternalApi"));
builder.Services.Configure<ExternalApiOptions>(
    builder.Configuration.GetSection("ETenderApi"));

// HTTP CLIENT
builder.Services.AddHttpClient("itender-api", (sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<InternalApiOptions>>().Value;

    client.BaseAddress = new Uri(config.BaseUrl);

    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddPolicyHandler(HttpPolicies.GetRetryPolicy())
.AddPolicyHandler(HttpPolicies.GetCircuitBreakerPolicy())
.AddPolicyHandler(HttpPolicies.GetTimeoutPolicy());

builder.Services.AddHttpClient("etender", (sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<ExternalApiOptions>>().Value;

    client.BaseAddress = new Uri(config.BaseUrl);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    if (!string.IsNullOrWhiteSpace(config.Username))
    {
        var credentials = Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}");
        var base64 = Convert.ToBase64String(credentials);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", base64);
    }
})
.AddPolicyHandler(HttpPolicies.GetRetryPolicy())
.AddPolicyHandler(HttpPolicies.GetCircuitBreakerPolicy())
.AddPolicyHandler(HttpPolicies.GetTimeoutPolicy());

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

builder.Services.AddSingleton<IETenderApiProvider, ETenderApiProvider>();
builder.Services.AddSingleton<IiTenderApiProvider, iTenderApiProvider>();
builder.Services.AddScoped<IComplianceCaseService, ComplianceCaseService>();
builder.Services.AddSingleton<IComplianceCaseRepository, ComplianceCaseRepository>();

builder.Services.AddSingleton<TenderProviderService>();

var host = builder.Build();
host.Run();
