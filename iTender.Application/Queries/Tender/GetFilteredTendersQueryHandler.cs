using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Tender
{
    public class GetFilteredTendersQueryHandler
    {
        private readonly ITenderRepository _repository;

        public GetFilteredTendersQueryHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<TenderModel>> Handle(GetFilteredTendersQuery query, CancellationToken ct) 
        {
            return _repository.GetByFilter(query.Filter, ct);
        }
    }
}
