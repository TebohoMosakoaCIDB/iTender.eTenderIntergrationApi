using iTender.Application.DTOs;

namespace iTender.Application.Queries.Tender
{
    public class GetAdvancedFilteredTenderQuery
    {
        public AdvancedTenderSearchViewModel Filter { get; }
        public GetAdvancedFilteredTenderQuery(AdvancedTenderSearchViewModel filter)
        {
                Filter = filter;
        }
    }
}
