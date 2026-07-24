using AutoMapper;
using iTender.Application.AutoMapperProfiles;
using iTender.Application.Commands.Application;
using iTender.Application.Commands.Contact;
using iTender.Application.Commands.Credentials;
using iTender.Application.Commands.Tender;
using iTender.Application.Queries.Application;
using iTender.Application.Queries.CdpSubmissions;
using iTender.Application.Queries.ConstructionContractContractor;
using iTender.Application.Queries.Contact;
using iTender.Application.Queries.Contract;
using iTender.Application.Queries.Contractor;
using iTender.Application.Queries.ContractorDevelopmentProgramme;
using iTender.Application.Queries.ContractorGrade;
using iTender.Application.Queries.Credentials;
using iTender.Application.Queries.FinancialStatement;
using iTender.Application.Queries.Tender;
using iTender.Application.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace iTender.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(DependencyInjection).Assembly);
            });

            services.AddScoped<GetApplicationByIdQueryHandler>(); // Application
            services.AddScoped<CreateApplicationCommandHandler>();
            services.AddScoped<GetContactByIdQueryHandler>(); // Contact
            services.AddScoped<CreateContactCommandHandler>();
            services.AddScoped<UpdateContactCommandHandler>();
            services.AddScoped<DeleteContactCommandHandler>();
            services.AddScoped<GetContactsByTenderIdQueryHandler>();
            services.AddScoped<GetCredentialByCredentialsQueryHandler>(); // Credentials
            services.AddScoped<GetCredentialByUsernameQueryHandler>();
            services.AddScoped<UpdateCredentialCommandHandler>();            
            services.AddScoped<GetConstructionContractContractorsByContractIdQueryHandler>(); // Construction Contract Contractors
            services.AddScoped<GetConstructionContractContractorsByContractorIdQueryHandler>();
            services.AddScoped<GetContractorByIdQueryHandler>(); // Contractor
            services.AddScoped<GetContractorByCrsNumberQueryHandler>();
            services.AddScoped<GetContractorsQueryHandler>();
            services.AddScoped<GetContractorGradesQueryHandler>(); // Contractor Grades
            services.AddScoped<GetFinancialStatementsByContractorHandler>(); // Financial Statement
            services.AddScoped<GetFinancialStatementsByApplicationIdHandler>();
            services.AddScoped<GetTenderCountByProvinceQueryHandler>(); // Tender
            services.AddScoped<GetTenderSummaryQueryHandler>();
            services.AddScoped<GetFilteredTendersQueryHandler>();
            services.AddScoped<GetAdvancedFilteredTenderQueryHandler>();
            services.AddScoped<GetTendersQueryHandler>();
            services.AddScoped<CreateTenderCommandHandler>();
            services.AddScoped<GetTenderByIdQueryHandler>();
            services.AddScoped<UpdateTenderCommandHandler>();
            services.AddScoped<UpdateFullTenderCommandHandler>();
            services.AddScoped<DeleteTenderCommandHandler>();
            services.AddScoped<GetAllTendersQueryHandler>();
            services.AddScoped<CheckForDuplicateContractNumbersQueryHandler>(); // Award
            services.AddScoped<AdvancedAwardSearchQueryHandler>();
            services.AddScoped<GetContractsAwardedQueryHandler>();
            services.AddScoped<GetContractByIdQueryHandler>();
            services.AddScoped<GetContractsByContractNumberQueryHandler>();
            services.AddScoped<GetContractsQueryHandler>();
            services.AddScoped<GetCDPByEmployerIdQueryHandler>(); //Contractor Development Programme
            services.AddScoped<GetCdpSubmissionsByCdpIdQueryHandler>(); //Contractor Development Programme Submission
            // Utilities
            services.AddScoped<GradingDesignationCalcUtil>();

            // Profiles
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CreateTenderMapper>();
            });

            return services;
        }
    }
}
