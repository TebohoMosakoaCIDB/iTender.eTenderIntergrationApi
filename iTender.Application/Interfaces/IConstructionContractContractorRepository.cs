using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IConstructionContractContractorRepository
    {
        Task<IEnumerable<ConstructionContractContractorModel>> GetByContractId(Guid contractId);
        Task<IEnumerable<ConstructionContractContractorModel>> GetByContractorId(Guid contractorId);
    }
}
