namespace iTender.Application.Queries.ContractorGrade
{
    public class GetContractorGradesQuery
    {
        public Guid ContractorId { get; set; }
        public Guid? ClassOfWorkTypeId { get; set; }
        public int? ApprovedGrade { get; set; }

        public GetContractorGradesQuery()
        {
        }

        public GetContractorGradesQuery(
            Guid contractorId,
            Guid? classOfWorkTypeId,
            int? approvedGrade)
        {
            ContractorId = contractorId;
            ClassOfWorkTypeId = classOfWorkTypeId;
            ApprovedGrade = approvedGrade;
        }
    }
}
