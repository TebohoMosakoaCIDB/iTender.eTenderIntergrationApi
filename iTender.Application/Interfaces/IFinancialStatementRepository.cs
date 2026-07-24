using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IFinancialStatementRepository
    {
        Task<List<FinancialStatementModel>> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct = default);
        Task<List<FinancialStatementModel>> GetByContractorAsync(Guid contractorId, CancellationToken ct = default);
    }
}
