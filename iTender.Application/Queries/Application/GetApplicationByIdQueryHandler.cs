using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Application
{
    public class GetApplicationByIdQueryHandler : IGetApplicationByIdQueryHandler
    {
        public readonly IApplicationRepository _repository;
        public GetApplicationByIdQueryHandler(IApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApplicationModel?> Handle(GetApplicationByIdQuery query, CancellationToken ct)
        {
            return await _repository.GetByIdAsync(query.Id, ct);
        }

    }
}