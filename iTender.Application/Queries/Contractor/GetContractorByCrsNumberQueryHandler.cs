using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contractor
{
    public class GetContractorByCrsNumberQueryHandler
    {
        private readonly IContractorRepository _repository;

        public GetContractorByCrsNumberQueryHandler(IContractorRepository repository)
        {
            _repository = repository;
        }

        public Task<ContractorModel?> Handle(
            GetContractorByCrsNumberQuery query,
            CancellationToken ct)
        {
            return _repository.GetByCrsNumber(query.CrsNumber, ct);
        }
    }
}
