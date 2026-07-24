using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.FinancialStatement
{
    public class GetFinancialStatementsByApplicationIdHandler
    {
        private readonly IFinancialStatementRepository _repository;

        public GetFinancialStatementsByApplicationIdHandler(
            IFinancialStatementRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FinancialStatementModel>> Handle(
            GetFinancialStatementsByApplicationIdQuery query,
            CancellationToken ct = default)
        {
            return await _repository.GetByApplicationIdAsync(query.ApplicationId, ct);
        }
    }
}
