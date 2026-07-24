using iTender.WorkerService.Enums;
using iTender.WorkerService.Models;
using iTender.WorkerService.Services;
using Serilog;

namespace iTender.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly Serilog.ILogger _logger;
        private readonly TenderProviderService _providerService;
        private readonly IComplianceCaseService _complianceCaseService;
        private readonly IComplianceCaseRepository _complianceCaseRepository;

        private static readonly HashSet<string> AllowedCategories = new()
        {
            "Construction",
            "Civil engineering",
            "Construction of buildings",
            "Services: Building"
        };

        public Worker(TenderProviderService providerService, IComplianceCaseService complianceCaseService, IComplianceCaseRepository complianceCaseRepository)
        {
            _logger = Log.ForContext<Worker>();
            _providerService = providerService;
            _complianceCaseService = complianceCaseService;
            _complianceCaseRepository = complianceCaseRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Tender Sync Worker started");

            var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            // Run immediately when the service starts
            await RunSync(stoppingToken);

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunSync(stoppingToken);
            }        
        }

        private async Task RunSync(CancellationToken ct)
        {
            try
            {
                // Simulate work first
                await Task.Delay(2000, ct);

                _logger.Information("Starting tender comparison...");

                // Get internal tenders
                var internalTenders = await _providerService.GetiTenderTendersAsync(ct);

                // Get external tenders
                var externalETenderTenders = await _providerService.GetAllAsync(ct);

                var filteredExternal = externalETenderTenders
                    .Where(x => !string.IsNullOrWhiteSpace(x.Category)
                        && AllowedCategories.Contains(x.Category))
                    .ToList();
               
                var internalTenderNumbers = new HashSet<string>(
                    internalTenders
                        .Where(t => !string.IsNullOrWhiteSpace(t.EmployerTenderNumber))
                        .Select(t => t.EmployerTenderNumber.Trim()),
                    StringComparer.OrdinalIgnoreCase);

                var classifiedTenders = new List<ClassifiedTender>();

                foreach (var externalTender in filteredExternal)
                {
                    var internalTender = internalTenders.FirstOrDefault(x =>
                        string.Equals(
                            x.EmployerTenderNumber,
                            externalTender.Number,
                            StringComparison.OrdinalIgnoreCase));

                    classifiedTenders.Add(new ClassifiedTender
                    {
                        Source = "eTender",
                        CheckedOn = DateTime.UtcNow,
                        ExternalTender = externalTender,
                        ComplianceStatus = internalTender == null
                            ? TenderComplianceStatus.NonCompliant
                            : TenderComplianceStatus.Compliant
                    });
                }

                //what to do with these tenders

                var compliantTenders = classifiedTenders
                    .Where(x => x.ComplianceStatus == TenderComplianceStatus.Compliant)
                    .ToList();

                var nonCompliantTenders = classifiedTenders
                    .Where(x => x.ComplianceStatus == TenderComplianceStatus.NonCompliant)
                    .ToList();

                await _complianceCaseService.ProcessNonCompliantTendersAsync(nonCompliantTenders, ct);

                await CheckOverdueCases(ct);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Sync failed");
            }
        }

        private async Task CheckOverdueCases(CancellationToken ct)
        {
            var openCases = await _complianceCaseRepository
                .GetOpenCasesAsync(ct);

            foreach (var caseItem in openCases)
            {
                if (caseItem.ResponseDueOn == null)
                    continue;

                if (caseItem.Status == ComplianceStatus.AwaitingResponse &&
                    DateTime.UtcNow > caseItem.ResponseDueOn)
                {
                    caseItem.Status = ComplianceStatus.Overdue;

                    await _complianceCaseRepository.UpdateAsync(caseItem, ct);

                    _logger.Warning(
                        "CASE OVERDUE: {TenderNumber}",
                        caseItem.TenderNumber);
                }
            }
        }
    }
}
