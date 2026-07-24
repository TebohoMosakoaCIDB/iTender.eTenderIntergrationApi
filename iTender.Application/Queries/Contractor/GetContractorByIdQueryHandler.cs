using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contractor
{
    public class GetContractorByIdQueryHandler
    {
        private readonly IContractorRepository _repository;

        public GetContractorByIdQueryHandler(IContractorRepository repository)
        {
            _repository = repository;
        }

        public Task<ContractorModel?> Handle(
            GetContractorByIdQuery query,
            CancellationToken ct)
        {
            return _repository.GetById(query.Id, ct);
        }
    }
}
