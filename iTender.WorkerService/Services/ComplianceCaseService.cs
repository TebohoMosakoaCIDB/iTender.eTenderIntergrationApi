using iTender.WorkerService.Enums;
using iTender.WorkerService.Models;

namespace iTender.WorkerService.Services
{
    public class ComplianceCaseService : IComplianceCaseService
    {
        private readonly IComplianceCaseRepository _repository;
        private readonly IInstructionLetterService _letterService;
        private readonly ILogger<ComplianceCaseService> _logger;

        public ComplianceCaseService(IComplianceCaseRepository repository, IInstructionLetterService letterService, ILogger<ComplianceCaseService> logger)
        {
            _repository = repository;
            _letterService = letterService;
            _logger = logger;
        }

        public async Task ProcessNonCompliantTendersAsync(List<ClassifiedTender> nonCompliantTenders, CancellationToken ct)
        {
            foreach (var item in nonCompliantTenders)
            {
                var tender = item.ExternalTender;

                if (string.IsNullOrWhiteSpace(tender.Number))
                    continue;

                var existingCase =
                    await _repository.GetByTenderNumberAsync(tender.Number, ct);

                if (existingCase == null)
                {
                    var newCase = new ComplianceCase
                    {
                        Id = Guid.NewGuid(),
                        TenderNumber = tender.Number,
                        TenderTitle = tender.Description,
                        Category = tender.Category,

                        Status = ComplianceStatus.Open,
                        DetectedOn = DateTime.UtcNow,
                        LastCheckedOn = DateTime.UtcNow,

                        ResponseDueOn = DateTime.UtcNow.AddHours(48),
                        LetterSentOn = DateTime.UtcNow,
                        LetterSent = true
                    };

                    await _repository.CreateAsync(newCase, ct);

                    await _letterService.SendLetterAsync(newCase, ct);

                    return;
                }

                existingCase.LastCheckedOn = DateTime.UtcNow;

                if (existingCase.LetterSentOn != null &&
                    existingCase.ResponseDueOn < DateTime.UtcNow &&
                    existingCase.Status == ComplianceStatus.AwaitingResponse)
                {
                    existingCase.Status = ComplianceStatus.Overdue;
                }

                await _repository.UpdateAsync(existingCase, ct);

                _logger.LogInformation(
                    "Compliance case UPDATED for {TenderNumber}",
                    tender.Number);
            }
        }
    }
}
