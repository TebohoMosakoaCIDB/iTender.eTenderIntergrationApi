using iTender.WorkerService.Models;

namespace iTender.WorkerService.Services
{
    public class InstructionLetterService : IInstructionLetterService
    {
        private readonly ILogger<InstructionLetterService> _logger;

        public InstructionLetterService(ILogger<InstructionLetterService> logger)
        {
            _logger = logger;
        }
        public Task SendLetterAsync(ComplianceCase complianceCase, CancellationToken ct)
        {
            _logger.LogInformation(
                "Sending instruction letter for tender {TenderNumber}",
                complianceCase.TenderNumber);

            // TODO:
            // 1. Generate PDF
            // 2. Email contractor
            // 3. Attach to record (Dataverse)

            return Task.CompletedTask;
        }
    }
}
