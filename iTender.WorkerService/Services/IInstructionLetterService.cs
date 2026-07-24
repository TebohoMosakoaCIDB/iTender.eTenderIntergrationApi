using iTender.WorkerService.Models;

namespace iTender.WorkerService.Services
{
    public interface IInstructionLetterService
    {
        Task SendLetterAsync(ComplianceCase complianceCase, CancellationToken ct);
    }
}
