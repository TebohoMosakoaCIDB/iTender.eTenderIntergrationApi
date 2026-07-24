using iTender.Application.DTOs;

namespace iTender.Application.Queries.Contract
{
    public class GetContractsQuery
    {
        public ContractFilterViewModel Filter { get; set; } = default!;
        public Guid EmployerId { get; set; }
    }
}
