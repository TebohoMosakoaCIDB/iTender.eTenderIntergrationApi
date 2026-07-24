namespace iTender.Application.Queries.Tender
{
    public class GetTenderByIdQuery
    {
        public Guid Id { get; set; }
        public GetTenderByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
