using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contract
{
    public class AdvancedAwardSearchQueryHandler
    {
        private readonly IContractRepository _repository;

        public AdvancedAwardSearchQueryHandler(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ContractModel>> Handle(AdvancedAwardSearchQuery query, CancellationToken ct)
        {
            var filter = query.Filter;

            return await _repository.AwardSearchAsync(query.Filter, ct);
        }
    }
}
