using iTender.Application.DTOs;

namespace iTender.Application.Queries.Contractor
{
    public class GetContractorsQuery
    {
        public ContractorFilterModel Filter { get; }

        public GetContractorsQuery(ContractorFilterModel filter)
        {
            Filter = filter;
        }
    }
}
