using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contractor
{
    public class GetContractorsQueryHandler
    {
        private readonly IContractorRepository _repository;

        public GetContractorsQueryHandler(IContractorRepository repository)
        {
            _repository = repository;
        }

        public Task<PagedResult<ContractorModel>> Handle(
            GetContractorsQuery query,
            CancellationToken ct)
        {
            return _repository.GetContractors(query.Filter, ct);
        }
    }
}
