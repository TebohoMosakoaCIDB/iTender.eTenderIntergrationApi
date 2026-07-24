namespace iTender.Application.Queries.FinancialStatement
{
    public class GetFinancialStatementsByApplicationIdQuery
    {
        public Guid ApplicationId { get; set; }

        public GetFinancialStatementsByApplicationIdQuery(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}
