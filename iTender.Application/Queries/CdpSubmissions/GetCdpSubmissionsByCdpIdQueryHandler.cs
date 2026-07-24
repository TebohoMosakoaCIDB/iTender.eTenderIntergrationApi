using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.CdpSubmissions
{
    public class GetCdpSubmissionsByCdpIdQueryHandler
    {
        private readonly ICdpSubmissionRepository _repository;

        public GetCdpSubmissionsByCdpIdQueryHandler(
            ICdpSubmissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CdpSubmissionModel>> Handle(
        GetCdpSubmissionsByCdpIdQuery request,
        CancellationToken cancellationToken)
        {
            return await _repository.GetByCdpIdAsync(
                request.Id,
                cancellationToken);
        }
    }
}
