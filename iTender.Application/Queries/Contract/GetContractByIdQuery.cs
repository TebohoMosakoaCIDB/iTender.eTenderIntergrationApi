namespace iTender.Application.Queries.Contract
{
    public class GetContractByIdQuery
    {
        public Guid Id { get; set; }

        public GetContractByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
