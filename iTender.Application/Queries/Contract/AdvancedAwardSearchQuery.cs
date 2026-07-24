using iTender.Application.DTOs;

namespace iTender.Application.Queries.Contract
{
    public class AdvancedAwardSearchQuery
    {
        public AdvancedAwardSearchModel Filter { get; }
        public AdvancedAwardSearchQuery(AdvancedAwardSearchModel filter)
        {
            Filter = filter;
        }
    }
}
