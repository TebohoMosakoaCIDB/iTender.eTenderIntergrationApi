using iTender.Application.Interfaces;
using iTender.Application.Providers;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Providers;
using iTender.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;

namespace iTender.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT Authentication
            

            // DI Registrations (Dataverse + Repos)

            services.AddOptions<CrmOptions>()
                .Bind(configuration.GetSection(CrmOptions.SectionName))
                .ValidateOnStart();

            services.AddOptions<EncryptionOptions>()
                .Bind(configuration.GetSection(EncryptionOptions.SectionName))
                .ValidateOnStart();

            services.AddHttpClient<IETenderApiProvider, ETenderApiProvider>((serviceProvider, client) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                client.BaseAddress = new Uri(configuration["ETenderApi:BaseUrl"]!);

                var username = configuration["ETenderApi:Username"];
                var password = configuration["ETenderApi:Password"];

                var credentials = $"{username}:{password}";
                var encodedCredentials = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(credentials));

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", encodedCredentials);

                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddScoped<ICrmServiceFactory, CrmServiceFactory>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<ICredentialRepository, CredentialRepository>();
            services.AddScoped<IConstructionContractContractorRepository, ConstructionContractContractorRepository>();
            services.AddScoped<IContractorRepository, ContractorRepository>();
            services.AddScoped<IContractorGradeRepository, ContractorGradeRepository>();
            services.AddScoped<IFinancialStatementRepository, FinancialStatementRepository>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<IGradingRepository, GradingRepository>();
            services.AddScoped<IJointVentureRepository, JointVentureRepository>();
            services.AddScoped<ITenderRepository, TenderRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IContractorDevelopmentProgrammeRepository, ContractorDevelopmentProgrammeRepository>();
            services.AddScoped<ICdpSubmissionRepository, CdpSubmissionRepository>();
            return services;
        }
    }
}