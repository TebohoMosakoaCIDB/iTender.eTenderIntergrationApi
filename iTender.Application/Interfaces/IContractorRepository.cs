using iTender.Application.DTOs;
using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IContractorRepository
    {
        Task<ContractorModel?> GetById(Guid id, CancellationToken ct = default);
        Task<ContractorModel?> GetByCrsNumber(string crsNumber, CancellationToken ct = default);
        Task<PagedResult<ContractorModel>> GetContractors(ContractorFilterModel filter, CancellationToken ct = default);
    }
}
