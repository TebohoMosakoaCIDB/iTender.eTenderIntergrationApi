using iTender.Application.Interfaces;

namespace iTender.Application.Queries.Contract
{
    public class CheckForDuplicateContractNumbersQueryHandler
    {
        private readonly IContractRepository _repository;

        public CheckForDuplicateContractNumbersQueryHandler(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            CheckForDuplicateContractNumbersQuery query,
            CancellationToken ct)
        {
            return _repository.CheckForDuplicateContractNumbers(query.EmployerId, query.ContractNumber, ct);
        }
    }
}
