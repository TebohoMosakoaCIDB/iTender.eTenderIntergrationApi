namespace iTender.Application.Queries.Contractor
{
    public class GetContractorByCrsNumberQuery
    {
        public string CrsNumber { get; }

        public GetContractorByCrsNumberQuery(string crsNumber)
        {
            CrsNumber = crsNumber;
        }
    }
}
