using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contract
{
    public class GetContractsByContractNumberQueryHandler
    {
        private readonly IContractRepository _repository;

        public GetContractsByContractNumberQueryHandler(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContractModel>> Handle(
            GetContractsByContractNumberQuery query,
            CancellationToken ct)
        {
            return _repository.GetContractsByContractNumber(
                query.EmployerId,
                query.ContractNumber,
                ct);
        }
    }
}
