using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IContractorGradeRepository
    {
        Task<IEnumerable<ContractorGradeModel>> GetContractorGrades(Guid contractorId, Guid? classOfWorkTypeId = null, int? approvedGrade = null, CancellationToken ct = default);
    }
}
