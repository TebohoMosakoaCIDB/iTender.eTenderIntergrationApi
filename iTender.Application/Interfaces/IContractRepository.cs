using iTender.Application.DTOs;
using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IContractRepository
    {
        bool CheckForDuplicateContractNumbers(Guid employerId, string contractNumber, CancellationToken ct = default);
        List<ContractModel> GetContractsByContractNumber(Guid employerId, string contractNumber, CancellationToken ct = default);
        List<ContractModel> GetContractsAwarded(Guid employerId, bool isPracticallyComplete, CancellationToken ct = default);
        Task<PagedResult<ContractModel>> AwardSearchAsync(AdvancedAwardSearchModel model, CancellationToken ct = default);
        ContractModel GetContractById(Guid contractId, CancellationToken ct = default);
        Task<PagedResult<ContractModel>> GetContracts(ContractFilterViewModel filter, Guid employerId, CancellationToken ct = default);
    }
}
