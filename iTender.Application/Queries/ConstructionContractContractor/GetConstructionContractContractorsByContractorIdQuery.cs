using System;
using System.Collections.Generic;
using System.Text;

namespace iTender.Application.Queries.ConstructionContractContractor
{
    public class GetConstructionContractContractorsByContractorIdQuery
    {
        public Guid ContractorId { get; }

        public GetConstructionContractContractorsByContractorIdQuery(Guid contractorId)
        {
            ContractorId = contractorId;
        }
    }
}
