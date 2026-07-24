namespace iTender.Application.Queries.FinancialStatement
{
    public class GetFinancialStatementsByContractorQuery
    {
        public Guid ContractorId { get; set; }

        public GetFinancialStatementsByContractorQuery(Guid contractorId)
        {
            ContractorId = contractorId;
        }
    }
}
