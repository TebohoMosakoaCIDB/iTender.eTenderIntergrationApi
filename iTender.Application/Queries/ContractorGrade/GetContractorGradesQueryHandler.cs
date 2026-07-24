using iTender.Application.Interfaces;
using iTender.Domain.Models;
namespace iTender.Application.Queries.ContractorGrade
{
    public class GetContractorGradesQueryHandler
    {
        private readonly IContractorGradeRepository _repository;

        public GetContractorGradesQueryHandler(IContractorGradeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContractorGradeModel>> Handle(GetContractorGradesQuery query, CancellationToken ct = default)
        {
            if (query == null)
                return Enumerable.Empty<ContractorGradeModel>();

            return await _repository.GetContractorGrades(
                query.ContractorId,
                query.ClassOfWorkTypeId,
                query.ApprovedGrade, ct
            );
        }
    }
}
