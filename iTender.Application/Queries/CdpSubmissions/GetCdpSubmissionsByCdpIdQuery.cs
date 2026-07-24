namespace iTender.Application.Queries.CdpSubmissions
{
    public class GetCdpSubmissionsByCdpIdQuery
    {
        public Guid Id { get; set; }
        public GetCdpSubmissionsByCdpIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
