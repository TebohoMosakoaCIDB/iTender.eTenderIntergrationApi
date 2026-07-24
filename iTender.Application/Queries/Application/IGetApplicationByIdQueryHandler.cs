using iTender.Domain.Models;

namespace iTender.Application.Queries.Application
{
    public interface IGetApplicationByIdQueryHandler
    {
        Task<ApplicationModel?> Handle(GetApplicationByIdQuery query, CancellationToken ct);
    }
}
