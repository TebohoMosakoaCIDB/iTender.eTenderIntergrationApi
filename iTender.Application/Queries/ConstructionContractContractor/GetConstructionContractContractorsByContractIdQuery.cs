namespace iTender.Application.Queries.ConstructionContractContractor
{
    public class GetConstructionContractContractorsByContractIdQuery
    {
        public Guid ContractId { get; }

        public GetConstructionContractContractorsByContractIdQuery(Guid contractId)
        {
            ContractId = contractId;
        }
    }
}
