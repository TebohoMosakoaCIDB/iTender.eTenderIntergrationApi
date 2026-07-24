namespace iTender.Application.Queries.Contract
{
    public class GetContractsByContractNumberQuery
    {
        public Guid EmployerId { get; set; }
        public string ContractNumber { get; set; }
        public GetContractsByContractNumberQuery(Guid employerId, string contractNumber)
        {
            EmployerId = employerId;
            ContractNumber = contractNumber;
        }
    }
}
