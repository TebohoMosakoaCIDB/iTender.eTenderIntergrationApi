using iTender.Application.DTOs;

namespace iTender.Application.Queries.Tender
{
    public class GetFilteredTendersQuery
    {
        public TenderFilterModel Filter { get; }

        public GetFilteredTendersQuery(TenderFilterModel filter)
        {
            Filter = filter;
        }
    }
}
