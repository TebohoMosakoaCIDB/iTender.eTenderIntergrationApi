using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.ConstructionContractContractor
{
    public class GetConstructionContractContractorsByContractorIdQueryHandler
    {
        private readonly IConstructionContractContractorRepository _repository;

        public GetConstructionContractContractorsByContractorIdQueryHandler(
            IConstructionContractContractorRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ConstructionContractContractorModel>> Handle(
            GetConstructionContractContractorsByContractorIdQuery query,
            CancellationToken ct)
        {
            return _repository.GetByContractorId(query.ContractorId);
        }
    }
}
