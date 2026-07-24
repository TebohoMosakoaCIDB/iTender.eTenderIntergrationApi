namespace iTender.Application.Queries.Contract
{
    public class GetContractsAwardedQuery
    {
        public Guid EmployerId { get; set; }
        public bool IsApisPracticallyComplete { get; set; }

        public GetContractsAwardedQuery(Guid employerId, bool isApisPracticallyComplete)
        {
            EmployerId = employerId;
            IsApisPracticallyComplete = isApisPracticallyComplete;
        }
    }
}
