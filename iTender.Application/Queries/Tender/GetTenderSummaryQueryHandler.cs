using iTender.Application.DTOs;
using iTender.Application.Interfaces;

namespace iTender.Application.Queries.Tender
{
    public class GetTenderSummaryQueryHandler
    {
        private readonly ITenderRepository _repository;

        public GetTenderSummaryQueryHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<TenderSummaryViewModel> Handle(CancellationToken ct)
        {
            return _repository.GetTenderSummary(ct);
        }
    }
}
