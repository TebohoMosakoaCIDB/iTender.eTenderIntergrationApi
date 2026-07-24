using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.FinancialStatement
{
    public class GetFinancialStatementsByContractorHandler
    {
        private readonly IFinancialStatementRepository _repository;

        public GetFinancialStatementsByContractorHandler(
            IFinancialStatementRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FinancialStatementModel>> Handle(
            GetFinancialStatementsByContractorQuery query,
            CancellationToken ct = default)
        {
            return await _repository.GetByContractorAsync(query.ContractorId, ct);
        }
    }
}
