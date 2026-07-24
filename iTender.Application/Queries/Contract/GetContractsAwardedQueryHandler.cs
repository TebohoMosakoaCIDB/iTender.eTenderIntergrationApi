using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contract
{
    public class GetContractsAwardedQueryHandler
    {
        private readonly IContractRepository _repository;

        public GetContractsAwardedQueryHandler(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContractModel>> Handle(
            GetContractsAwardedQuery query,
            CancellationToken ct)
        {
            return _repository.GetContractsAwarded(
                query.EmployerId,
                query.IsApisPracticallyComplete,
                ct);
        }
    }
}
