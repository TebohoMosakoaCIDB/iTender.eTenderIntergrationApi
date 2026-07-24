using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.ContractorDevelopmentProgramme
{
    public class GetCDPByEmployerIdQueryHandler
    {
        private readonly IContractorDevelopmentProgrammeRepository _repository;

        public GetCDPByEmployerIdQueryHandler(IContractorDevelopmentProgrammeRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ContractorDevelopmentProgrammeModel>> Handle(GetCDPByEmployerIdQuery query, CancellationToken ct)
        {
            return await _repository.GetAllCDPsForEmployer(query.Filter);
        }
    }
}
