namespace iTender.Application.Queries.Application
{
    public class GetApplicationByIdQuery
    {
        public Guid Id { get; set; }

        public GetApplicationByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
