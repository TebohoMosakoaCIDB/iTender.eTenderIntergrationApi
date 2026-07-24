using iTender.WorkerService.Models;

namespace iTender.WorkerService.Services
{
    public class ComplianceCaseRepository : IComplianceCaseRepository
    {        
        public Task CreateAsync(ComplianceCase caseItem, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ComplianceCase?> GetByTenderNumberAsync(string tenderNumber, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<ComplianceCase>> GetOpenCasesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ComplianceCase caseItem, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
