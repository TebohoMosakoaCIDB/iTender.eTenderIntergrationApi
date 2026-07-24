namespace iTender.Application.Queries.Contract
{
    public class CheckForDuplicateContractNumbersQuery
    {
        public Guid EmployerId { get; set; }
        public string ContractNumber { get; set; }

        public CheckForDuplicateContractNumbersQuery(Guid employerId, string contractNumber)
        {
            
            EmployerId = employerId;
            ContractNumber = contractNumber;
        }
    }
}
