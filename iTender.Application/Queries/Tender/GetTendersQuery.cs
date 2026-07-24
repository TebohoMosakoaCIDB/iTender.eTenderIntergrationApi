using iTender.Application.DTOs;
using iTender.Domain.Enums;

namespace iTender.Application.Queries.Tender
{
    public class GetTendersQuery
    {
        public TenderFilterViewModel Filter { get; set; } = default!;
        public Guid EmployerId { get; set; }
        public TenderType TenderType { get; set; }
    }
}
