using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Tender
{
    public class GetAdvancedFilteredTenderQueryHandler
    {
        private readonly ITenderRepository _repository;

        public GetAdvancedFilteredTenderQueryHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<TenderModel>> Handle(GetAdvancedFilteredTenderQuery query, CancellationToken ct)
        {
            return _repository.GetAdvancedFilteredTenders(query.Filter, ct);
        }
    }
}
