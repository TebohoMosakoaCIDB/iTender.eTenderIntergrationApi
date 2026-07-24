using iTender.Application.DTOs;
using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IContractorDevelopmentProgrammeRepository
    {
        Task<PagedResult<ContractorDevelopmentProgrammeModel>> GetAllCDPsForEmployer(CDPViewModel filter, CancellationToken ct = default);
    }
}
