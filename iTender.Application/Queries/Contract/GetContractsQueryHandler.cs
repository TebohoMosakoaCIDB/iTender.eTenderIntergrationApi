using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contract
{
    public class GetContractsQueryHandler
    {
        private readonly IContractRepository _repository;
        public GetContractsQueryHandler(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ContractModel>> Handle(GetContractsQuery query, CancellationToken ct)
        {
            return await _repository.GetContracts(
                query.Filter,
                query.EmployerId,
                ct);
        }
    }
}
