using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Tender
{
    public class GetAllTendersQueryHandler
    {
        private readonly ITenderRepository _repository;

        public GetAllTendersQueryHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public List<TenderModel> Handle(GetAllTendersQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            query.CancellationToken.ThrowIfCancellationRequested();

            return _repository.GetTendersAsync(query.CancellationToken);
        }
    }
}
