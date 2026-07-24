namespace iTender.Application.Queries.Contractor
{
    public class GetContractorByIdQuery
    {
        public Guid Id { get; }

        public GetContractorByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
