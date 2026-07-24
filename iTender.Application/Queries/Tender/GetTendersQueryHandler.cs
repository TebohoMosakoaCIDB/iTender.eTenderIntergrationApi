using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Tender
{
    public class GetTendersQueryHandler
    {
        private readonly ITenderRepository _repository;

        public GetTendersQueryHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<TenderModel>> Handle(
            GetTendersQuery request,
            CancellationToken ct)
        {
            return await _repository.GetTenders(
                request.Filter,
                request.EmployerId,
                request.TenderType,
                ct);
        }
    }
}
