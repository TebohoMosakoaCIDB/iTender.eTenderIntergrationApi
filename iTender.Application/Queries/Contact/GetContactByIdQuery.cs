namespace iTender.Application.Queries.Contact
{
    public class GetContactByIdQuery
    {
        public Guid Id { get; set; }

        public GetContactByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
