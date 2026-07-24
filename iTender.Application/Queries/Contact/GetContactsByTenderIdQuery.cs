namespace iTender.Application.Queries.Contact
{
    public class GetContactsByTenderIdQuery
    {
        public Guid Id { get; set; }

        public GetContactsByTenderIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
