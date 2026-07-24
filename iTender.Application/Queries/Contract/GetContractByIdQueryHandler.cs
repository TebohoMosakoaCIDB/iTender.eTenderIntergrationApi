using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contract
{
    public class GetContractByIdQueryHandler
    {
        private readonly IContractRepository _repository;

        public GetContractByIdQueryHandler(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContractModel> Handle(
            GetContractByIdQuery query,
            CancellationToken ct)
        {
            return _repository.GetContractById(
                query.Id,
                ct);
        }
    }
}
