using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.ConstructionContractContractor
{
    public class GetConstructionContractContractorsByContractIdQueryHandler
    {
        private readonly IConstructionContractContractorRepository _repository;

        public GetConstructionContractContractorsByContractIdQueryHandler(
            IConstructionContractContractorRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ConstructionContractContractorModel>> Handle(
            GetConstructionContractContractorsByContractIdQuery query,
            CancellationToken ct)
        {
            return _repository.GetByContractId(query.ContractId);
        }
    }
}
