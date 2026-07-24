using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Tender
{
    public class GetTenderByIdQueryHandler
    {
        private readonly ITenderRepository _repository;
        public GetTenderByIdQueryHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<TenderModel> Handle(GetTenderByIdQuery query, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(query.Id, cancellationToken);
        }
    }
}
