namespace iTender.Application.Queries.Tender
{
    public class GetAllTendersQuery
    {
        public CancellationToken CancellationToken { get; }

        public GetAllTendersQuery(CancellationToken cancellationToken = default)
        {
            CancellationToken = cancellationToken;
        }
    }
}
